using MediatR;

namespace MTAA_Backend.Application.CQRS.Comments.Commands
{
    public class DislikeComment : IRequest
    {
        public Guid CommentId { get; set; }
    }
}
