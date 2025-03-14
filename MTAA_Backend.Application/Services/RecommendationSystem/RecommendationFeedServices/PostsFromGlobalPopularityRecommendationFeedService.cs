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
    public class PostsFromGlobalPopularityRecommendationFeedService : IPostsFromGlobalPopularityRecommendationFeedService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IRecommendationItemsService _recommendationItemsService;

        public const int MaxFeedCount = 200;
        public const int HoursAgo = 2;

        public const int LikesScore = 1;
        public const int CommentsScore = 4;

        public const double Weight = 0.1;
        public PostsFromGlobalPopularityRecommendationFeedService(MTAA_BackendDbContext dbContext,
            IRecommendationItemsService recommendationItemsService)
        {
            _dbContext = dbContext;
            _recommendationItemsService = recommendationItemsService;
        }

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