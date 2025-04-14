using MediatR;

namespace MTAA_Backend.Application.CQRS.Comments.Commands
{
    public class EditComment : IRequest
    {
        public Guid CommentId { get; set; }
        public string Text { get; set; }
    }
}
