using MediatR;

namespace MTAA_Backend.Application.CQRS.Comments.Events
{
    public class DeleteCommentEvent : INotification
    {
        public Guid CommentId { get; set; }
    }
}
