using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.DTOs.RecommendationSystem.Requests;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Posts.RecommendationSystem;
using MTAA_Backend.Infrastructure;
using System.Threading;

namespace MTAA_Backend.Application.Services.RecommendationSystem
{
    public class RecommendationItemsService : IRecommendationItemsService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IStringLocalizer<ErrorMessages> _localizer;
        private readonly ILogger _logger;

        public RecommendationItemsService(MTAA_BackendDbContext dbContext,
            IStringLocalizer<ErrorMessages> localizer,
            ILogger<RecommendationItemsService> logger)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task AddPostsToLocalFeed(RecommendationFeedTypes feedType, string userId, ICollection<SimpleAddRecommendationItemRequest> requests, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.Where(e => e.Id == userId)
                                             .Include(e => e.WatchedPosts)
                                             .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                _logger.LogError($"User {userId} not found");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound]);
            }

            var allFeeds = await _dbContext.LocalRecommendationFeeds.Where(e => e.UserId == userId)
                                                                   .Include(e => e.RecommendationItems)
                                                                   .ToListAsync(cancellationToken);

            var feed = allFeeds.FirstOrDefault(e => e.Type == feedType);

            if (feed == null)
            {
                _logger.LogError($"Recommendation feed not found userId: {userId}, type: {feedType}");
                throw new HttpException("Feed not found");
            }

            foreach (var request in requests)
            {
                if (user.WatchedPosts.Any(e => e.Id == request.PostId)) continue;

                bool flag = false;
                foreach (var userFeed in allFeeds)
                {
                    if (userFeed.RecommendationItems.Any(e => e.PostId == request.PostId))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag) continue;

                var newItem = new RecommendationItem
                {
                    FeedId = feed.Id,
                    PostId = request.PostId,
                    LocalScore = request.LocalScore
                };
                _dbContext.RecommendationItems.Add(newItem);
                feed.RecommendationItems.Add(newItem);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RemovePostsFromLocalFeed(RecommendationFeedTypes feedType, string userId, ICollection<Guid> postIds, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.Where(e => e.Id == userId)
                                             .Include(e => e.WatchedPosts)
                                             .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                _logger.LogError($"User {userId} not found");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound]);
            }

            var feed = await _dbContext.LocalRecommendationFeeds.Where(e => e.UserId == userId && e.Type == feedType)
                                                          .Include(e => e.RecommendationItems)
                                                          .FirstOrDefaultAsync(cancellationToken);

            if (feed == null)
            {
                _logger.LogError($"Recommendation feed not found userId: {userId}, type: {feedType}");
                throw new HttpException("Feed not found");
            }

            foreach(var postId in postIds)
            {
                var item = feed.RecommendationItems.FirstOrDefault(e => e.PostId == postId);
                if (item != null)
                {
                    feed.RecommendationItems.Remove(item);
                    _dbContext.RecommendationItems.Remove(item);
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddPostsToSharedFeed(RecommendationFeedTypes feedType, ICollection<SimpleAddRecommendationItemRequest> requests, CancellationToken cancellationToken = default)
        {
            if (requests.Count == 0) return;

            var feed = await _dbContext.SharedRecommendationFeeds.Where(e => e.Type == feedType)
                                                                .Include(e => e.RecommendationItems)
                                                                .FirstOrDefaultAsync(cancellationToken);

            if (feed == null)
            {
                _logger.LogError($"Recommendation feed not found: {feedType}");
                throw new HttpException("Feed not found");
            }

            bool flag = false;
            foreach (var request in requests)
            {
                if (feed.RecommendationItems.Any(e => e.PostId == request.PostId)) continue;
                flag = true;
                feed.RecommendationItemsCount++;
                var newItem = new RecommendationItem
                {
                    FeedId = feed.Id,
                    PostId = request.PostId,
                    LocalScore = request.LocalScore
                };
                _dbContext.RecommendationItems.Add(newItem);
                feed.RecommendationItems.Add(newItem);
            }

            if (flag)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task ClearSharedFeed(RecommendationFeedTypes feedType, CancellationToken cancellationToken = default)
        {
            var feed = await _dbContext.SharedRecommendationFeeds.Where(e => e.Type == feedType)
                                                                .Include(e => e.RecommendationItems)
                                                                .FirstOrDefaultAsync(cancellationToken);

            if (feed == null)
            {
                _logger.LogError($"Recommendation feed not found: {feedType}");
                throw new HttpException("Feed not found");
            }
            foreach(var item in feed.RecommendationItems)
            {
                _dbContext.RecommendationItems.Remove(item);
            }
            feed.RecommendationItemsCount = 0;
            feed.RecommendationItems.Clear();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task RemovePostsFromSharedFeed(RecommendationFeedTypes feedType, string userId, ICollection<Guid> postIds, CancellationToken cancellationToken = default)
        {
            var feed = await _dbContext.SharedRecommendationFeeds.Where(e => e.Type == feedType)
                                                                .Include(e => e.RecommendationItems)
                                                                .FirstOrDefaultAsync(cancellationToken);

            if (feed == null)
            {
                _logger.LogError($"Recommendation feed not found: {feedType}");
                throw new HttpException("Feed not found");
            }

            foreach (var postId in postIds)
            {
                var item = feed.RecommendationItems.FirstOrDefault(e => e.PostId == postId);
                if (item != null)
                {
                    feed.RecommendationItems.Remove(item);
                    _dbContext.RecommendationItems.Remove(item);
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
