using MediatR;
using MTAA_Backend.Domain.Resources.Notifications;

namespace MTAA_Backend.Application.CQRS.Notifications.Commands
{
    public class AddNotification : IRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public NotificationType Type { get; set; }
        public string UserId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? CommentId { get; set; }
    }
}
