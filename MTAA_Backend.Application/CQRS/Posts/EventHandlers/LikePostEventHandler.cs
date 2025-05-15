using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Notifications.Commands;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Application.Services.RecommendationSystem.RecommendationFeedServices;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Notifications;
using MTAA_Backend.Infrastructure;
using Nest;

namespace MTAA_Backend.Application.CQRS.Posts.EventHandlers
{
    public class LikePostEventHandler(
        MTAA_BackendDbContext _dbContext,
        IMediator _mediator,
        IPostsFromPreferencesRecommendationFeedService _recService,
        IUserService _userService) : INotificationHandler<LikePostEvent>
    {
        public async Task Handle(LikePostEvent notification, CancellationToken cancellationToken)
        {
            var postLike = await _dbContext.PostLikes
                .Include(pl => pl.User)
                .Include(pl => pl.Post)
                    .ThenInclude(p => p.Owner)
                .FirstOrDefaultAsync(pl => pl.Id == notification.PostLikeId, cancellationToken);

            if (postLike == null || postLike.Post.Owner.Id == postLike.User.Id)
                return;

            var userId = _userService.GetCurrentUserId();
            await _recService.ChangeReccommendations(userId, postLike.PostId,(float)PostsFromGlobalPopularityRecommendationFeedService.LikesScore/100f,  cancellationToken: cancellationToken);

            await _mediator.Send(new AddNotification
            {
                Title = "New Like",
                Text = $"{postLike.User.DisplayName} liked your post",
                Type = NotificationType.LikePost,
                UserId = postLike.Post.Owner.Id,
                PostId = postLike.Post.Id,
                CommentId = null
            }, cancellationToken);
        }
    }
}
