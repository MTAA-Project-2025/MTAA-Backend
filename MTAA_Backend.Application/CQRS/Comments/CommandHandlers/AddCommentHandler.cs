using MediatR;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Comments.Commands;
using MTAA_Backend.Domain.Entities.Posts.Comments;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.CommandHandlers
{
    public class AddCommentHandler(ILogger<AddCommentHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService) : IRequestHandler<AddComment, Guid>
    {
        public async Task<Guid> Handle(AddComment request, CancellationToken cancellationToken)
        {
            var comment = new Comment()
            {
                PostId = request.PostId,
                ParentCommentId = request.ParentCommentId,
                Text = request.Text,
                OwnerId = _userService.GetCurrentUserId()
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return comment.Id;
        }
    }
}
