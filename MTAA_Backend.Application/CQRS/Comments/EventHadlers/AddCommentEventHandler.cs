using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Comments.Events;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.EventHadlers
{
    public class AddCommentEventHandler(MTAA_BackendDbContext _dbContext) : INotificationHandler<AddCommentEvent>
    {
        public async Task Handle(AddCommentEvent notification, CancellationToken cancellationToken)
        {
            if (notification.ParentCommentId == null) return;

            var parentId = notification.ParentCommentId;

            while (parentId != null)
            {
                var parentComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == parentId, cancellationToken);
                if (parentComment != null)
                {
                    parentComment.ChildCommentsCount++;
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
