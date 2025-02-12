using Ardalis.GuardClauses;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Guards.Groups
{
    public static class UserUpdateUserGroupMembershipGuard
    {
        public static async Task NotUserGroupMembershipOwner(this IGuardClause guardClause, Guid membershipId, MTAA_BackendDbContext dbContext, IStringLocalizer localizer, IUserService userService)
        {
            var membership = await dbContext.UserGroupMemberships.FindAsync(membershipId);
            if (membership == null) throw new HttpException(localizer[ErrorMessagesPatterns.UserGroupMembershipNotFound], System.Net.HttpStatusCode.NotFound);

            var ownerId = userService.GetCurrentUserId();
            if (ownerId == null) throw new HttpException(localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.BadRequest);

            if (membership.UserId != ownerId) throw new HttpException(localizer[ErrorMessagesPatterns.UserDontHaveRules], HttpStatusCode.BadRequest);
        }
    }
}
