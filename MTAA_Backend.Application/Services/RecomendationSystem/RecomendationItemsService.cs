using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.DTOs.RecomendationSystem.Requests;
using MTAA_Backend.Domain.Entities.Posts.RecomendationSystem;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces.RecomendationSystem;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Posts.RecomendationSystem;
using MTAA_Backend.Infrastructure;
using System.Threading;

namespace MTAA_Backend.Application.Services.RecomendationSystem
{
    public class RecomendationItemsService : IRecomendationItemsService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IStringLocalizer<ErrorMessages> _localizer;
        private readonly ILogger<RecomendationItemsService> _logger;

        public RecomendationItemsService(MTAA_BackendDbContext dbContext,
            IStringLocalizer<ErrorMessages> localizer,
            ILogger<RecomendationItemsService> logger)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task AddPostsToFeed(RecomendationFeedTypes feedType, string userId, ICollection<SimpleAddRecomendationItemRequest> requests, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.Where(e => e.Id == userId)
                                             .Include(e => e.WatchedPosts)
                                             .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                _logger.LogError($"User {userId} not found");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound]);
            }

            var feed = await _dbContext.RecomendationFeeds.Where(e => e.UserId == userId && e.Type == feedType)
                                                          .Include(e => e.RecomendationItems)
                                                          .FirstOrDefaultAsync(cancellationToken);

            var allFeeds = await _dbContext.RecomendationFeeds.Where(e => e.UserId == userId)
                                                             .Include(e => e.RecomendationItems)
                                                             .ToListAsync(cancellationToken);

            if (feed == null)
            {
                _logger.LogError($"Recomendation feed not found userId: {userId}, type: {feedType}");
                throw new HttpException("Feed not found");
            }

            foreach (var request in requests)
            {
                if (user.WatchedPosts.Any(e => e.Id == request.PostId)) continue;

                bool flag = false;
                foreach (var userFeed in allFeeds)
                {
                    if (userFeed.RecomendationItems.Any(e => e.PostId == request.PostId))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag) continue;

                feed.RecomendationItems.Add(new RecomendationItem
                {
                    FeedId = feed.Id,
                    PostId = request.PostId,
                    LocalScore = request.LocalScore
                });
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RemovePostsFromFeed(RecomendationFeedTypes feedType, string userId, ICollection<Guid> postIds, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.Where(e => e.Id == userId)
                                             .Include(e => e.WatchedPosts)
                                             .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                _logger.LogError($"User {userId} not found");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound]);
            }

            var feed = await _dbContext.RecomendationFeeds.Where(e => e.UserId == userId && e.Type == feedType)
                                                          .Include(e => e.RecomendationItems)
                                                          .FirstOrDefaultAsync(cancellationToken);

            if (feed == null)
            {
                _logger.LogError($"Recomendation feed not found userId: {userId}, type: {feedType}");
                throw new HttpException("Feed not found");
            }

            foreach(var postId in postIds)
            {
                var item = feed.RecomendationItems.FirstOrDefault(e => e.PostId == postId);
                if (item != null)
                {
                    feed.RecomendationItems.Remove(item);
                    _dbContext.RecomendationItems.Remove(item);
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
