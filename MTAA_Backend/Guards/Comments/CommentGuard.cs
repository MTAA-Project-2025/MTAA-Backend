using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.Services;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Guards.Comments
{
    public static class CommentGuard
    {
        public static async Task NotCommentOwner(this IGuardClause guardClause, Guid commentId, MTAA_BackendDbContext dbContext, IUserService userService, IStringLocalizer<ErrorMessages> localizer)
        {
            var comment = await dbContext.Comments.Include(c => c.Post).FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null)
            {
                throw new HttpException(localizer[ErrorMessagesPatterns.CommentNotFound], HttpStatusCode.NotFound);
            }

            var currentUserId = userService.GetCurrentUserId();
            if (comment.OwnerId != currentUserId && comment.Post.OwnerId != currentUserId)
            {
                throw new HttpException(localizer[ErrorMessagesPatterns.UserDontHaveRules], HttpStatusCode.Forbidden);
            }
        }
    }
}
