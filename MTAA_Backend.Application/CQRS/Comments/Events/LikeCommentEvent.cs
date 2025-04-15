using MediatR;

namespace MTAA_Backend.Application.CQRS.Comments.Events
{
    public class LikeCommentEvent : INotification
    {
        public Guid CommentId { get; set; }
    }
}
