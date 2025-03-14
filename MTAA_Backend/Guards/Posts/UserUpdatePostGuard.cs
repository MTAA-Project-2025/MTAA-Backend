using Ardalis.GuardClauses;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Guards.Posts
{
    public static class UserUpdatePostGuard
    {
        public static async Task NotPostOwner(this IGuardClause guardClause, Guid postId, MTAA_BackendDbContext dbContext, IStringLocalizer localizer, IUserService userService)
        {
            var channel = await dbContext.Posts.FindAsync(postId);
            if (channel == null) throw new HttpException(localizer[ErrorMessagesPatterns.PostNotFound], System.Net.HttpStatusCode.NotFound);

            var ownerId = userService.GetCurrentUserId();
            if (ownerId == null) throw new HttpException(localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.BadRequest);

            if (channel.OwnerId != ownerId) throw new HttpException(localizer[ErrorMessagesPatterns.UserDontHaveRules], HttpStatusCode.BadRequest);
        }
    }
}
