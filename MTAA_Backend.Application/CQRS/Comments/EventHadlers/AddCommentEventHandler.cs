using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Comments.Events;
using MTAA_Backend.Application.CQRS.Notifications.Commands;
using MTAA_Backend.Application.Services.RecommendationSystem.RecommendationFeedServices;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Notifications;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.EventHadlers
{
    /// <summary>
    /// Handles the <see cref="AddCommentEvent"/> to update post and comment statistics,
    /// trigger recommendation system updates, and send notifications.
    /// </summary>
    public class AddCommentEventHandler(MTAA_BackendDbContext _dbContext,
        IMediator _mediator,
        IUserService _userService,
        IPostsFromPreferencesRecommendationFeedService _recService) : INotificationHandler<AddCommentEvent>
    {
        /// <summary>
        /// The maximum length for displaying comment text in notifications.
        /// </summary>
        readonly int maxLength = 200;

        /// <summary>
        /// Handles the <see cref="AddCommentEvent"/> asynchronous.
        /// </summary>
        /// <param name="notification">The <see cref="AddCommentEvent"/> notification.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Handle(AddCommentEvent notification, CancellationToken cancellationToken)
        {
            var currentUser = await _userService.GetCurrentUser();
            if (notification.PostId == null) return;
            var post = await _dbContext.Posts.Where(e => e.Id == notification.PostId).FirstOrDefaultAsync(cancellationToken);
            if (post == null || currentUser == null) return;
            post.CommentsCount++;
            post.GlobalScore += PostsFromGlobalPopularityRecommendationFeedService.CommentsScore;

            var userId = _userService.GetCurrentUserId();
            await _recService.ChangeReccommendations(userId, post.Id, (float)PostsFromGlobalPopularityRecommendationFeedService.CommentsScore / 100f, cancellationToken: cancellationToken);

            if (notification.ParentCommentId == null)
            {
                await _mediator.Send(new AddNotification()
                {
                    PostId = post.Id,
                    Title = $"New comment",
                    Text = $"{currentUser.DisplayName} added new comment. " + ((notification.Text.Length <= maxLength) ? notification.Text : notification.Text.Substring(0, maxLength)),
                    UserId = post.OwnerId,
                    Type = NotificationType.WriteCommentOnPost
                });

                return;
            }

            var parentId = notification.ParentCommentId;

            while (parentId != null)
            {
                var parentComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == parentId, cancellationToken);
                if (parentComment != null)
                {
                    parentComment.ChildCommentsCount++;
                    parentId = parentComment.ParentCommentId;
                }
                else
                {
                    parentId = null;
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);

            var firstparentComment = await _dbContext.Comments.Where(c => c.Id == notification.ParentCommentId).Include(e => e.Owner).FirstOrDefaultAsync(cancellationToken);
            if (firstparentComment == null || currentUser == null) return;

            await _mediator.Send(new AddNotification()
            {
                CommentId = notification.CommentId,
                Title = $"New reply",
                Text = $"{currentUser.DisplayName} added new reply. " + ((notification.Text.Length <= maxLength) ? notification.Text : notification.Text.Substring(0, maxLength)),
                UserId = firstparentComment.OwnerId,
                Type = NotificationType.WriteCommentAsAnswer
            });
        }
    }
}
