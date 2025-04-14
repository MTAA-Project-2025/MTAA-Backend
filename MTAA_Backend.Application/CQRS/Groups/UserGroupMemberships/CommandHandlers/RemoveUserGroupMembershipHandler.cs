using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.CommandHandlers
{
    internal class RemoveUserGroupMembershipHandler(ILogger<RemoveUserGroupMembershipHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext) : IRequestHandler<RemoveUserGroupMembership>
    {
        public async Task Handle(RemoveUserGroupMembership request, CancellationToken cancellationToken)
        {
            var membership = await _dbContext.UserGroupMemberships.Where(e => e.Id == request.Id)
                                                                  .Include(e => e.User)
                                                                  .Include(e => e.Group)
                                                                  .FirstOrDefaultAsync(cancellationToken);
            if (membership == null)
            {
                _logger.LogError($"User group membership not found: {request.Id}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserGroupMembershipNotFound], HttpStatusCode.NotFound);
            }

            _dbContext.Remove(membership);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
