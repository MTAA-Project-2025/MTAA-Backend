using MediatR;
using MTAA_Backend.Application.CQRS.Users.Account.Events;
using MTAA_Backend.Application.CQRS.Versions.Command;
using MTAA_Backend.Domain.Resources.Versioning;

namespace MTAA_Backend.Application.CQRS.Users.Account.EventHandlers
{
    public class AccountUpdateEventHandler(IMediator _mediator) : INotificationHandler<AccountUpdateEvent>
    {
        public async Task Handle(AccountUpdateEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.Send(new IncreaseVersion
            {
                UserId = notification.UserId,
                VersionItemType = VersionItemType.Account
            }, cancellationToken);
        }
    }
}
