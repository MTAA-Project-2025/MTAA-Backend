using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MTAA_Backend.Application.CQRS.Comments.Events;
using MTAA_Backend.Application.CQRS.Notifications.Commands;
using MTAA_Backend.Domain.Resources.Notifications;
using MTAA_Backend.Infrastructure;
using Nest;

namespace MTAA_Backend.Application.CQRS.Comments.EventHadlers
{
    public class LikeCommentEventHandler(IMediator _mediator,
        MTAA_BackendDbContext _dbContext) : INotificationHandler<LikeCommentEvent>
    {
        readonly int maxLength = 200;
        public async Task Handle(LikeCommentEvent notification, CancellationToken cancellationToken)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(e=>e.Id==notification.CommentId, cancellationToken);
            if (comment == null) return;

            await _mediator.Send(new AddNotification()
            {
                CommentId = notification.CommentId,
                Title = "New like",//Todo: add localization
                Text = "Someone liked your comment. " + (comment.Text.Length <= maxLength ? comment.Text : comment.Text.Substring(0, maxLength)),
                UserId = comment.OwnerId,
                Type = NotificationType.LikeComment
            });
        }
    }
}
