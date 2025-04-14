using MediatR;

namespace MTAA_Backend.Application.CQRS.Notifications.Events
{
    public class AddNotificationEvent : INotification
    {
        public Guid NotificationId { get; set; }
        public string UserId { get; set; }
    }
}
