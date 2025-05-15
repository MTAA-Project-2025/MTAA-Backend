using MediatR;
using MTAA_Backend.Application.CQRS.Notifications.Commands;
using MTAA_Backend.Application.CQRS.Notifications.Events;
using MTAA_Backend.Domain.Entities.Notifications;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Notifications.CommandHandlers
{
    public class AddNotificationHandler(MTAA_BackendDbContext _dbContext,
        IMediator _mediator,
        IUserService _userService) : IRequestHandler<AddNotification>
    {
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
