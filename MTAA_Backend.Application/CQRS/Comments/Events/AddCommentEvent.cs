using MediatR;

namespace MTAA_Backend.Application.CQRS.Comments.Events
{
    public class AddCommentEvent : INotification
    {
        public string Text { get; set; }
        public Guid? PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public Guid CommentId { get; set; }
    }
}
