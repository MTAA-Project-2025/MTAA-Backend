using MediatR;

namespace MTAA_Backend.Application.CQRS.Comments.Events
{
    public class AddCommentEvent : INotification
    {
        public Guid? ParentCommentId { get; set; }
    }
}
