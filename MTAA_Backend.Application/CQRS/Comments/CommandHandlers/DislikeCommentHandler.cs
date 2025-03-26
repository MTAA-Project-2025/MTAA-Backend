using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Comments.Commands;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.CommandHandlers
{
    public class DislikeCommentCommandHandler(MTAA_BackendDbContext _dbContext) : IRequestHandler<DislikeComment>
    {
        public async Task Handle(DislikeComment request, CancellationToken cancellationToken)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken);
            if (comment != null)
            {
                comment.DislikesCount++;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
