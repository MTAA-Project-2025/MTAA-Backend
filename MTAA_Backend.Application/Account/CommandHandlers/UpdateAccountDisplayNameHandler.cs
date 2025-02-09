using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.Account.Commands;
using MTAA_Backend.Application.Identity.CommandHandlers;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Account.CommandHandlers
{
    public class UpdateAccountDisplayNameHandler(MTAA_BackendDbContext dbContext,
        IUserService userService) : IRequestHandler<UpdateAccountDisplayName>
    {
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IUserService _userService = userService;

        public async Task Handle(UpdateAccountDisplayName request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetCurrentUser();
            user.DisplayName = request.DisplayName;
            user.IsEdited = true;
            user.DataLastEditTime = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
