using MediatR;
using MTAA_Backend.Application.CQRS.Versions.Command;
using MTAA_Backend.Application.CQRS.Versions.Events;
using MTAA_Backend.Domain.Entities.Versions;

namespace MTAA_Backend.Application.CQRS.Versions.EventHandlers
{
    public class AccountUpdatedEventHandler(IMediator _mediator) : INotificationHandler<AccountUpdatedEvent>
    {
        public async Task Handle(AccountUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.Send(new IncreaseVersion { UserId = notification.UserId, VersionItemType = notification.VersionItemType }, cancellationToken);
        }
    }
}
