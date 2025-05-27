using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.DTOs.RecommendationSystem.Requests;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Posts.RecommendationSystem;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.Services.RecommendationSystem.RecommendationFeedServices
{
    /// <summary>
    /// Provides services for managing recommendation feeds based on globally popular posts.
    /// </summary>
    public class PostsFromGlobalPopularityRecommendationFeedService : IPostsFromGlobalPopularityRecommendationFeedService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IRecommendationItemsService _recommendationItemsService;

        /// <summary>
        /// The maximum number of posts to include in the feed.
        /// </summary>
        public const int MaxFeedCount = 200;

        /// <summary>
        /// The number of hours to consider for recent post interactions.
        /// </summary>
        public const int HoursAgo = 2;

        /// <summary>
        /// The score assigned to each like on a post.
        /// </summary>
        public const int LikesScore = 1;

        /// <summary>
        /// The score assigned to each comment on a post.
        /// </summary>
        public const int CommentsScore = 4;

        /// <summary>
        /// The weight assigned to the global popularity feed.
        /// </summary>
        public const double Weight = 0.1;

        /// <summary>
        /// Initializes a new instance of the PostsFromGlobalPopularityRecommendationFeedService class.
        /// </summary>
        /// <param name="dbContext">The database context for data operations.</param>
        /// <param name="recommendationItemsService">The service for managing recommendation items.</param>
        public PostsFromGlobalPopularityRecommendationFeedService(MTAA_BackendDbContext dbContext,
            IRecommendationItemsService recommendationItemsService)
        {
            _dbContext = dbContext;
            _recommendationItemsService = recommendationItemsService;
        }

        /// <summary>
        /// Adds a global popularity recommendation feed for a user if it does not exist.
        /// </summary>
        /// <param name="userId">The ID of the user to add the feed for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddFeed(string userId, CancellationToken cancellationToken = default)
        {
            var feed = await _dbContext.SharedRecommendationFeeds.Where(e => e.Type == RecommendationFeedTypes.PostsFromGlobalPopularityFeed)
                                                                .FirstOrDefaultAsync(cancellationToken);
            if (feed == null)
            {
                feed = new SharedRecommendationFeed()
                {
                    Type = RecommendationFeedTypes.PostsFromGlobalPopularityFeed,
                    Weight = Weight
                };
                _dbContext.SharedRecommendationFeeds.Add(feed);
            }

            var user = await _dbContext.Users.Where(e => e.Id == userId)
                                             .Include(e => e.SharedRecommendationFeeds)
                                             .FirstOrDefaultAsync(cancellationToken);

            if(user!=null && !user.SharedRecommendationFeeds.Any(e => e.Type == feed.Type))
            {
                user.SharedRecommendationFeeds.Add(feed);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Background job to recommend posts based on global popularity scores.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RecomendPostsBackgroundJob(CancellationToken cancellationToken = default)
        {
            DateTime time = DateTime.UtcNow.AddHours(-HoursAgo);
            var posts = await _dbContext.Posts.Select(p => new
            {
                Post = p,
                Score = p.Likes.Count(l => l.DataCreationTime >= time) * LikesScore + p.Comments.Count(c => c.DataCreationTime >= time) * CommentsScore
            }).OrderByDescending(p => p.Score)
              .Take(MaxFeedCount)
              .ToListAsync(cancellationToken);

            var request = new List<SimpleAddRecommendationItemRequest>(MaxFeedCount);
            foreach (var post in posts)
            {
                request.Add(new SimpleAddRecommendationItemRequest
                {
                    PostId = post.Post.Id,
                    LocalScore = post.Score
                });
            }
            await _recommendationItemsService.ClearSharedFeed(RecommendationFeedTypes.PostsFromGlobalPopularityFeed, cancellationToken);
            await _recommendationItemsService.AddPostsToSharedFeed(RecommendationFeedTypes.PostsFromGlobalPopularityFeed, request, cancellationToken);
        }
    }
}