using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Users.Account.CommandHandlers
{
    /// <summary>
    /// Handles the <see cref="SaveFirebaseToken"/> command to save a Firebase Cloud Messaging (FCM) token for a user.
    /// This token is typically used to send push notifications to the user's device.
    /// </summary>
    public class SaveFirebaseTokenHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService) : IRequestHandler<SaveFirebaseToken>
    {
        /// <summary>
        /// Handles the <see cref="SaveFirebaseToken"/> command.
        /// </summary>
        /// <param name="request">The <see cref="SaveFirebaseToken"/> command request, containing the Firebase token.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Handle(SaveFirebaseToken request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var firebaseItem = await _dbContext.FirebaseItems.Where(e => e.UserId == userId && e.Token == request.Token).FirstOrDefaultAsync(cancellationToken);
            if (firebaseItem != null) return;

            var newFirebaseItem = new FirebaseItem()
            {
                UserId = userId,
                Token = request.Token,
            };
            _dbContext.FirebaseItems.Add(newFirebaseItem);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
