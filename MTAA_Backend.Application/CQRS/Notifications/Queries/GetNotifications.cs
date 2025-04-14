using MediatR;
using MTAA_Backend.Domain.DTOs.Notifications.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Resources.Notifications;
using System.Collections;

namespace MTAA_Backend.Application.CQRS.Notifications.Queries
{
    public class GetNotifications : IRequest<ICollection<NotificationResponse>>
    {
        public NotificationType? Type { get; set; }
        public PageParameters PageParameters { get; set; }
    }
}
