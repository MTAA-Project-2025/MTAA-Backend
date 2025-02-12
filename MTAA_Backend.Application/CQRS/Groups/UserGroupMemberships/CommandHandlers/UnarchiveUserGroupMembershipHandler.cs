using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.CommandHandlers
{
    public class UnarchiveUserGroupMembershipHandler(ILogger<UnarchiveUserGroupMembershipHandler> logger,
        IStringLocalizer<ErrorMessages> localizer,
        MTAA_BackendDbContext dbContext) : IRequestHandler<UnarchiveUserGroupMembership>
    {
        private readonly ILogger _logger = logger;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly MTAA_BackendDbContext _dbContext = dbContext;

        public async Task Handle(UnarchiveUserGroupMembership request, CancellationToken cancellationToken)
        {
            var membership = await _dbContext.UserGroupMemberships.Where(e => e.Id == request.Id)
                                                                  .Include(e => e.User)
                                                                  .FirstOrDefaultAsync(cancellationToken);

            if (membership == null)
            {
                _logger.LogError($"User group membership not found: {request.Id}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserGroupMembershipNotFound], HttpStatusCode.NotFound);
            }

            membership.IsArchived = false;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
