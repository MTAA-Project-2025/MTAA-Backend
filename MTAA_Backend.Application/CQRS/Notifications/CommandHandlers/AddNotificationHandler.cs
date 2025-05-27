using MediatR;
using MTAA_Backend.Application.CQRS.Notifications.Commands;
using MTAA_Backend.Application.CQRS.Notifications.Events;
using MTAA_Backend.Domain.Entities.Notifications;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Notifications.CommandHandlers
{
    /// <summary>
    /// Handles the <see cref="AddNotification"/> command to create and persist a new notification.
    /// </summary>
    public class AddNotificationHandler(MTAA_BackendDbContext _dbContext,
        IMediator _mediator,
        IUserService _userService) : IRequestHandler<AddNotification>
    {
        /// <summary>
        /// Handles the <see cref="AddNotification"/> command.
        /// </summary>
        /// <param name="request">The <see cref="AddNotification"/> command request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Handle(AddNotification request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == request.UserId) return;

            var notification = new Notification
            {
                Title = request.Title,
                Text = request.Text,
                Type = request.Type,
                UserId = request.UserId,
                PostId = request.PostId,
                CommentId = request.CommentId
            };

            await _dbContext.Notifications.AddAsync(notification, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            await _mediator.Publish(new AddNotificationEvent
            {
                NotificationId = notification.Id,
                UserId = notification.UserId
            });
            
        }
    }
}
