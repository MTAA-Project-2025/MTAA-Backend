using MediatR;

namespace MTAA_Backend.Application.CQRS.Comments.Commands
{
    public class LikeComment : IRequest
    {
        public Guid CommentId { get; set; }
    }
}
