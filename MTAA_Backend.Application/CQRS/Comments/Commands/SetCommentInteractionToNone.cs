using MediatR;

namespace MTAA_Backend.Application.CQRS.Comments.Commands
{
    public class SetCommentInteractionToNone : IRequest
    {
        public Guid CommentId { get; set; }
    }
}
