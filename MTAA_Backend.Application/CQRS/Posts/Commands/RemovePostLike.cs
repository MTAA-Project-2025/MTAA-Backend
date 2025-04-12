using MediatR;

namespace MTAA_Backend.Application.CQRS.Posts.Commands
{
    public class RemovePostLike : IRequest
    {
        public Guid Id { get; set; }
    }
}
