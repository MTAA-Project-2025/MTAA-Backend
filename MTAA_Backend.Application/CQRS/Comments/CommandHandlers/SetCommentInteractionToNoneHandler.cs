using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Comments.Commands;
using MTAA_Backend.Domain.Entities.Posts.Comments;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Comments;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Comments.CommandHandlers
{
    public class SetCommentInteractionToNoneHandler(
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService) : IRequestHandler<SetCommentInteractionToNone>
    {
        public async Task Handle(SetCommentInteractionToNone request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var comment = await _dbContext.Comments
                .FirstOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken);

            if (comment == null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.CommentNotFound], HttpStatusCode.NotFound);
            }

            var interaction = await _dbContext.CommentInteractions
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.CommentId == request.CommentId, cancellationToken);

            if (interaction != null)
            {
                if (interaction.Type == CommentInteractionType.Like)
                {
                    comment.LikesCount--;
                }
                else if (interaction.Type == CommentInteractionType.Dislike)
                {
                    comment.DislikesCount--;
                }

                interaction.Type = CommentInteractionType.None;

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
