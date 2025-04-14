using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Notifications.Events;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Notifications.Responses;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Infrastructure;
using MTAA_Backend.Infrastructure.Hubs;

namespace MTAA_Backend.Application.CQRS.Notifications.EventHandlers
{
    public class CreateNotificationEventHandler(
        MTAA_BackendDbContext _dbContext,
        IHubContext<NotificationHub> _hubContext,
        IMapper _mapper) : INotificationHandler<NotificationCreatedEvent>
    {
        public async Task Handle(NotificationCreatedEvent notification, CancellationToken cancellationToken)
        {
            var dbNotification = await _dbContext.Notifications
                .Include(n => n.Post)
                    .ThenInclude(p => p.Images)
                        .ThenInclude(ig => ig.Images)
                .Include(n => n.Comment)
                .FirstOrDefaultAsync(n => n.Id == notification.NotificationId, cancellationToken);

            if (dbNotification == null)
                return;

            var notificationResponse = _mapper.Map<NotificationResponse>(dbNotification);

            if (dbNotification.Post?.Images != null && dbNotification.Post.Images.Any())
            {
                var smallImg = dbNotification.Post.Images.First().Images.First(e => e.Type == ImageSizeType.Small);
                notificationResponse.Image = _mapper.Map<MyImageResponse>(smallImg);
            }

            await _hubContext.Clients.User(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", notificationResponse, cancellationToken);
        }
    }
}
