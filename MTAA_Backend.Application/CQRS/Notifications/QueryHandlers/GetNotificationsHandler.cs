using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Notifications.Queries;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Notifications.Responses;
using MTAA_Backend.Domain.Entities.Notifications;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Infrastructure;
using System.Collections;
using System.Linq.Expressions;

namespace MTAA_Backend.Application.CQRS.Notifications.QueryHandlers
{
    public class GetNotificationsHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper) : IRequestHandler<GetNotifications, ICollection<NotificationResponse>>
    {
        public async Task<ICollection<NotificationResponse>> Handle(GetNotifications request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            Expression<Func<Notification, bool>> filterCondition = n => n.UserId == userId;

            if (request.Type != null)
            {
                filterCondition = filterCondition.And(n => n.Type == request.Type);
            }

            var notifications = await _dbContext.Notifications
                .Where(filterCondition)
                .OrderByDescending(n => n.DataCreationTime)
                .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                .Take(request.PageParameters.PageSize)
                .Include(n => n.Post)
                    .ThenInclude(p => p.Images)
                        .ThenInclude(ig => ig.Images)
                .Include(n => n.Comment)
                .Select(n => new
                {
                    Id = n.Id,
                    Title = n.Title,
                    Text = n.Text,
                    Type = n.Type,
                    DataCreationTime = n.DataCreationTime,
                    PostId = n.PostId,
                    CommentId = n.CommentId,
                    UserId = n.UserId,
                    Image = n.Post != null && n.Post.Images.Any() ?
                        n.Post.Images.First().Images.Where(i => i.Type == ImageSizeType.Small).FirstOrDefault() : null
                })
                .ToListAsync(cancellationToken);

            var mappedNotifications = new List<NotificationResponse>(notifications.Count);

            foreach (var notification in notifications)
            {
                mappedNotifications.Add(new NotificationResponse()
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Text = notification.Text,
                    Type = notification.Type,
                    DataCreationTime = notification.DataCreationTime,
                    PostId = notification.PostId,
                    CommentId = notification.CommentId,
                    UserId = notification.UserId,
                    Image = notification.Image != null ?
                        _mapper.Map<MyImageResponse>(notification.Image) : null
                });
            }

            return mappedNotifications;
        }
    }
}
