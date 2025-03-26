using MediatR;

namespace MTAA_Backend.Application.CQRS.Comments.Commands
{
    public class AddComment : IRequest<Guid>
    {
        public Guid PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string Text { get; set; }
    }
}
