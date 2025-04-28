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
    public class PostsFromFollowersRecommendationFeedService : IPostsFromFollowersRecommendationFeedService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IStringLocalizer<ErrorMessages> _localizer;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public const double Weight = 0.3;

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
