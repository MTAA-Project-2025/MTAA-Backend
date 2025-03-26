using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Comments.Commands;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.CommandHandlers
{
    public class LikeCommentHandler(MTAA_BackendDbContext _dbContext) : IRequestHandler<LikeComment>
    {
        public async Task Handle(LikeComment request, CancellationToken cancellationToken)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken);
            if (comment != null)
            {
                comment.LikesCount++;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
