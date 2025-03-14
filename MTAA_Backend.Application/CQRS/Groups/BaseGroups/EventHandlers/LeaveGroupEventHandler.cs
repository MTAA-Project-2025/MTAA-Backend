using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.CommandHandlers;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Commands;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Events;
using MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.BaseGroups.EventHandlers
{
    public class LeaveGroupEventHandler(ILogger<LeaveGroupEventHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IMediator _mediator) : INotificationHandler<LeaveGroupEvent>
    {
        public async Task Handle(LeaveGroupEvent notification, CancellationToken cancellationToken)
        {
            var membership = await _dbContext.UserGroupMemberships.FirstOrDefaultAsync(e => e.GroupId == notification.GroupId && e.UserId == notification.UserId, cancellationToken);
            if (membership == null)
            {
                _logger.LogError($"User group membership not found userId: {notification.UserId}, groupId: {notification.GroupId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserGroupMembershipNotFound], HttpStatusCode.NotFound);
            }

            await _mediator.Send(new RemoveUserGroupMembership()
            {
                Id = membership.Id
            }, cancellationToken);
        }
    }
}
