using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Comments.Commands;
using MTAA_Backend.Application.CQRS.Comments.Events;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Comments.CommandHandlers
{
    public class DeleteCommentHandler(ILogger<DeleteCommentHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMediator _mediator) : IRequestHandler<DeleteComment>
    {
        public async Task Handle(DeleteComment request, CancellationToken cancellationToken)
        {
            var comment = await _dbContext.Comments.Where(e => e.Id == request.CommentId)
                                                   .Include(e => e.ChildComments)
                                                   .FirstOrDefaultAsync(cancellationToken);
            if (comment == null)
            {
                _logger.LogError($"Comment {request.CommentId} not found or permission denied");
                throw new HttpException(_localizer[ErrorMessagesPatterns.CommentNotFound], HttpStatusCode.NotFound);
            }

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new DeleteCommentEvent()
            {
                ChildCommentsCount = comment.ChildComments.Count,
                ParentCommentId = comment.ParentCommentId,
                PostId = comment.PostId
            });
        }
    }
}
