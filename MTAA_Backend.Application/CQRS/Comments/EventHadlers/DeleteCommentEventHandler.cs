using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Comments.Events;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.EventHadlers
{
    public class DeleteCommentEventHandler(MTAA_BackendDbContext _dbContext) : INotificationHandler<DeleteCommentEvent>
    {
        public async Task Handle(DeleteCommentEvent notification, CancellationToken cancellationToken)
        {
            int decreaseBy = notification.ChildCommentsCount + 1;
            var parentId = notification.ParentCommentId;

            while (parentId != null)
            {
                var parentComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == parentId, cancellationToken);
                if (parentComment != null)
                {
                    parentComment.ChildCommentsCount -= decreaseBy;
                    parentId = parentComment.ParentCommentId;
                }
                else
                {
                    parentId = null;
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
