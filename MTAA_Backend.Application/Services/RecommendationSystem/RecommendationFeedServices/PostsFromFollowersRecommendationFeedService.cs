using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Posts.RecommendationSystem;
using MTAA_Backend.Infrastructure;
using System.CodeDom;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Services.RecommendationSystem.RecommendationFeedServices
{
    /// <summary>
    /// Provides services for managing recommendation feeds based on posts from followed users.
    /// </summary>
    public class PostsFromFollowersRecommendationFeedService : IPostsFromFollowersRecommendationFeedService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IStringLocalizer<ErrorMessages> _localizer;
        private readonly IBackgroundJobClient _backgroundJobClient;

        /// <summary>
        /// The weight assigned to the posts from followers feed.
        /// </summary>
        public const double Weight = 0.3;

        /// <summary>
        /// Initializes a new instance of the PostsFromFollowersRecommendationFeedService class.
        /// </summary>
        /// <param name="dbContext">The database context for data operations.</param>
        /// <param name="logger">The logger for recording errors.</param>
        /// <param name="localizer">The localizer for error messages.</param>
        /// <param name="backgroundJobClient">The client for scheduling background jobs.</param>
        public PostsFromFollowersRecommendationFeedService(MTAA_BackendDbContext dbContext,
            ILogger<PostsFromFollowersRecommendationFeedService> logger,
            IStringLocalizer<ErrorMessages> localizer,
            IBackgroundJobClient backgroundJobClient)
        {
            _dbContext = dbContext;
            _logger = logger;
            _localizer = localizer;
            _backgroundJobClient = backgroundJobClient;
        }

        /// <summary>
        /// Adds a posts-from-followers recommendation feed for a user if it does not exist.
        /// </summary>
        /// <param name="userId">The ID of the user to add the feed for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddFeed(string userId, CancellationToken cancellationToken = default)
        {
            var feed = await _dbContext.LocalRecommendationFeeds.Where(e => e.UserId == userId && e.Type == RecommendationFeedTypes.PostsFromFollowersFeed)
                                                               .FirstOrDefaultAsync(cancellationToken);
            if (feed != null) return;

            feed = new LocalRecommendationFeed()
            {
                UserId = userId,
                IsActive = true,
                Type = RecommendationFeedTypes.PostsFromFollowersFeed,
                Weight = Weight
            };
            _dbContext.LocalRecommendationFeeds.Add(feed);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Recommends a post to a user's followers by scheduling a background job.
        /// </summary>
        /// <param name="postId">The ID of the post to recommend.</param>
        /// <param name="userId">The ID of the user whose followers will receive the recommendation.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="HttpException">Thrown if the post or user is not found.</exception>
        public async Task RecomendPost(Guid postId, string userId, CancellationToken cancellationToken = default)
        {
            var post = await _dbContext.Posts.FindAsync(postId, cancellationToken);
            if (post == null)
            {
                _logger.LogError($"Post {postId} not found");
                throw new HttpException(_localizer[ErrorMessagesPatterns.PostNotFound], HttpStatusCode.NotFound);
            }

            var user = await _dbContext.Users.FindAsync(userId, cancellationToken);
            if (user == null)
            {
                _logger.LogError($"User {userId} not found");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound], HttpStatusCode.NotFound);
            }

            _backgroundJobClient.Enqueue(() => RecomendPostBackgroundJob(postId, userId, cancellationToken));
        }

        /// <summary>
        /// Background job to add a post recommendation to the feeds of a user's followers.
        /// </summary>
        /// <param name="postId">The ID of the post to recommend.</param>
        /// <param name="userId">The ID of the user whose followers will receive the recommendation.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RecomendPostBackgroundJob(Guid postId, string userId, CancellationToken cancellationToken = default)
        {
            var feeds = await _dbContext.UserRelationships.Where(e => (e.User2Id == userId && e.IsUser1Following) ||
                                                                      (e.User1Id == userId && e.IsUser2Following))
                                                          .Include(e => e.User1.LocalRecommendationFeeds.Where(f => f.Type == RecommendationFeedTypes.PostsFromFollowersFeed))
                                                              .ThenInclude(f => f.RecommendationItems)
                                                          .Include(e => e.User2.LocalRecommendationFeeds.Where(f => f.Type == RecommendationFeedTypes.PostsFromFollowersFeed))
                                                              .ThenInclude(f => f.RecommendationItems)
                                                          .Select(e => e.User1Id == userId ? e.User2.LocalRecommendationFeeds : e.User1.LocalRecommendationFeeds)
                                                          .ToListAsync(cancellationToken);

            foreach (var feedCollection in feeds)
            {
                if (feedCollection == null) continue;
                BaseRecommendationFeed feed = feedCollection.First(e => e.Type == RecommendationFeedTypes.PostsFromFollowersFeed);
                feed.RecommendationItemsCount++;

                var RecommendationItem = new RecommendationItem
                {
                    PostId = postId,
                    Feed = feed,
                    LocalScore = 1
                };
                _dbContext.RecommendationItems.Add(RecommendationItem);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
