using MediatR;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.CommandHandlers;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Commands;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Events;
using MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Groups.BaseGroups.EventHandlers
{
    public class JoinGroupEventHandler(IMediator _mediator) : INotificationHandler<JoinGroupEvent>
    {
        public async Task Handle(JoinGroupEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.Send(new AddUserGroupMembership()
            {
                UserId = notification.UserId,
                GroupId = notification.GroupId
            }, cancellationToken); 
        }
    }
}
