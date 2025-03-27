using MediatR;

namespace MTAA_Backend.Application.CQRS.Posts.Commands
{
    public class AddPostLike : IRequest
    {
        public Guid Id { get; set; }
    }
}
