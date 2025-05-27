using MediatR;
using MTAA_Backend.Application.CQRS.Users.Account.Events;
using MTAA_Backend.Application.CQRS.Versions.Command;
using MTAA_Backend.Domain.Resources.Versioning;

namespace MTAA_Backend.Application.CQRS.Users.Account.EventHandlers
{
    /// <summary>
    /// Handles the <see cref="AccountUpdateEvent"/> to signal that a user's account data has been updated,
    /// triggering a version increase for the account.
    /// </summary>
    public class AccountUpdateEventHandler(IMediator _mediator) : INotificationHandler<AccountUpdateEvent>
    {
        /// <summary>
        /// Handles the <see cref="AccountUpdateEvent"/> asynchronously.
        /// </summary>
        /// <param name="notification">The <see cref="AccountUpdateEvent"/> notification.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
