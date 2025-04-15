using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Notifications.Events;
using MTAA_Backend.Application.CQRS.Versions.Command;
using MTAA_Backend.Application.Services.Notifications;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Notifications.Responses;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Versioning;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Notifications.EventHandlers
{
    public class AddNotificationEventHandler(
        MTAA_BackendDbContext _dbContext,
        ISSEClientStorage _clientStorage,
        IMapper _mapper,
        IMediator _mediator) : INotificationHandler<AddNotificationEvent>
    {
        public async Task Handle(AddNotificationEvent notification, CancellationToken cancellationToken)
        {
            var dbNotification = await _dbContext.Notifications
                .Where(e=>e.Id==notification.NotificationId)
                .Include(n => n.Post)
                    .ThenInclude(p => p.Images)
                        .ThenInclude(ig => ig.Images)
                .Include(n => n.Comment)
                .FirstOrDefaultAsync(cancellationToken);

            if (dbNotification == null)
                return;

            var notificationResponse = _mapper.Map<NotificationResponse>(dbNotification);

            if (dbNotification.Post?.Images != null && dbNotification.Post.Images.Any())
            {
                var smallImg = dbNotification.Post.Images.First().Images.First(e => e.Type == ImageSizeType.Small);
                notificationResponse.Image = _mapper.Map<MyImageResponse>(smallImg);
            }

            await _clientStorage.SendNotificationAsync(notification.UserId, notificationResponse);

            await _mediator.Send(new IncreaseVersion()
            {
                UserId = dbNotification.UserId,
                VersionItemType = VersionItemType.Notifications,
            });
        }
    }
}
