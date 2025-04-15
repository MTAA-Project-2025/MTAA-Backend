using MediatR;

namespace MTAA_Backend.Application.CQRS.Comments.Events
{
    public class DeleteCommentEvent : INotification
    {
        public int ChildCommentsCount { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
