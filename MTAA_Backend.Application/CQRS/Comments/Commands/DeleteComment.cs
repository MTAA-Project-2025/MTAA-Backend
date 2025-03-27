using MediatR;

namespace MTAA_Backend.Application.CQRS.Comments.Commands
{
    public class DeleteComment : IRequest
    {
        public Guid CommentId { get; set; }
    }
}
