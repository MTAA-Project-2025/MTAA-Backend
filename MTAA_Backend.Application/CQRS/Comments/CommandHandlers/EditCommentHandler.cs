using MediatR;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Comments.Commands;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Comments.CommandHandlers
{
    public class EditCommentHandler(ILogger<EditCommentHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService) : IRequestHandler<EditComment>
    {
        public async Task Handle(EditComment request, CancellationToken cancellationToken)
        {
            var comment = await _dbContext.Comments.FindAsync(request.CommentId, cancellationToken);
            if (comment == null)
            {
                _logger.LogError($"Comment {request.CommentId} not found or permission denied");
                throw new HttpException(_localizer[ErrorMessagesPatterns.ResourceNotFound], HttpStatusCode.NotFound);
            }

            comment.Text = request.NewText;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

}
