using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Comments.Commands;
using MTAA_Backend.Application.CQRS.Comments.Events;
using MTAA_Backend.Domain.Entities.Posts.Comments;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Comments.CommandHandlers
{
    /// <summary>
    /// Handles the AddComment command, creating a new comment or a reply to an existing comment.
    /// </summary>
    public class AddCommentHandler(ILogger<AddCommentHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMediator _mediator) : IRequestHandler<AddComment, Guid>
    {
        /// <summary>
        /// Handles the AddComment command.
        /// </summary>
        /// <param name="request">The AddComment command request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The ID of the newly created comment.</returns>
        /// <exception cref="HttpException">Thrown if the post or parent comment is not found.</exception>
        public async Task<Guid> Handle(AddComment request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(e => e.Id == request.PostId, cancellationToken);
            if (post == null)
            {
                _logger.LogError($"Post not found {request.PostId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.PostNotFound], HttpStatusCode.BadRequest);
            }

            if (request.ParentCommentId != null)
            {
                var parentComment = await _dbContext.Comments.FirstOrDefaultAsync(e=>e.Id==request.ParentCommentId, cancellationToken);

                if (parentComment == null)
                {
                    _logger.LogError($"Parent comment not found {request.ParentCommentId}");
                    throw new HttpException(_localizer[ErrorMessagesPatterns.CommentNotFound], HttpStatusCode.NotFound);
                }
            }
            var comment = new Comment()
            {
                PostId = request.PostId,
                ParentCommentId = request.ParentCommentId,
                Text = request.Text,
                OwnerId = _userService.GetCurrentUserId()
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new AddCommentEvent()
            {
                ParentCommentId = request.ParentCommentId,
                CommentId = comment.Id,
                PostId = request.PostId,
                Text = request.Text
            });

            return comment.Id;
        }
    }
}
