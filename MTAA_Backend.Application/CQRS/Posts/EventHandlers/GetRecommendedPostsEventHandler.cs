using MediatR;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Infrastructure;
using MTAA_Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MTAA_Backend.Application.CQRS.Posts.EventHandlers
{
    public class GetRecommendedPostsEventHandler(MTAA_BackendDbContext _dbContext,
        IVectorDatabaseRepository _vectorDatabaseRepository) : INotificationHandler<GetRecommendedPostsEvent>
    {
        public async Task Handle(GetRecommendedPostsEvent notification, CancellationToken cancellationToken)
        {
            var postIds = notification.Posts.Select(e => e.Id).ToList();
            var items = _dbContext.RecommendationItems.Where(e => postIds.Contains(e.PostId));
            _dbContext.RecommendationItems.RemoveRange(items);

            var user = await _dbContext.Users.Where(e => e.Id == notification.UserId)
                                             .Include(e => e.WatchedPosts)
                                             .FirstOrDefaultAsync(cancellationToken);

            foreach (var post in notification.Posts)
            {
                await _vectorDatabaseRepository.UpdatePostWatched(post.Id, notification.UserId, cancellationToken);
                user.WatchedPosts.Add(post);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
