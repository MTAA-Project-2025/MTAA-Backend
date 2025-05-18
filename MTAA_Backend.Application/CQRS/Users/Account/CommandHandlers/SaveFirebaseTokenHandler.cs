using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Users.Account.CommandHandlers
{
    public class SaveFirebaseTokenHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService) : IRequestHandler<SaveFirebaseToken>
    {
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
