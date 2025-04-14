using MediatR;

namespace MTAA_Backend.Application.CQRS.Notifications.Events
{
    public class NotificationCreatedEvent : INotification
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
    }
}
