using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.CQRS.Users.Account.CommandHandlers
{
    public class UpdateAccountUsernameHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService) : IRequestHandler<UpdateAccountUsername>
    {
        public async Task Handle(UpdateAccountUsername request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetCurrentUser();
            user.UserName = request.Username;
            user.IsEdited = true;
            user.DataLastEditTime = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
