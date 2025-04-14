using MediatR;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.CommandHandlers
{
    internal class AddUserGroupMembershipHandler(ILogger<AddUserGroupMembershipHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext) : IRequestHandler<AddUserGroupMembership, Guid>
    {
        public async Task<Guid> Handle(AddUserGroupMembership request, CancellationToken cancellationToken)
        {
            var group = await _dbContext.BaseGroups.FindAsync(request.GroupId, cancellationToken);
            if (group == null)
            {
                _logger.LogError($"Group not found: {request.GroupId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.GroupNotFound], HttpStatusCode.NotFound);
            }

            var user = await _dbContext.Users.FindAsync(request.UserId);
            if (user == null)
            {
                _logger.LogError($"User not found: {request.UserId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound], HttpStatusCode.NotFound);
            }

            var userGroupMembership = new UserGroupMembership()
            {
                Group = group,
                User = user
            };

            _dbContext.UserGroupMemberships.Add(userGroupMembership);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return userGroupMembership.Id;
        }
    }
}
