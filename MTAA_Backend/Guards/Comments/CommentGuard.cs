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
        public static async Task NotCommentOwner(this IGuardClause guardClause, Guid commentId, MTAA_BackendDbContext dbContext, IStringLocalizer localizer, IUserService userService)
        {
            var comment = await dbContext.Comments.Where(e => e.Id == commentId).Include(c => c.Post).FirstOrDefaultAsync();
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
