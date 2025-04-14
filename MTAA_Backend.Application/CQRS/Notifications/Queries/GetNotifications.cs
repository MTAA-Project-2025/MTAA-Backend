using MediatR;
using MTAA_Backend.Domain.DTOs.Notifications.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;

namespace MTAA_Backend.Application.CQRS.Notifications.Queries
{
    public class GetNotifications : IRequest<List<NotificationResponse>>
    {
        public PageParameters PageParameters { get; set; }
    }
}
