using MediatR;

namespace MTAA_Backend.Application.CQRS.Posts.Commands
{
    public class DeletePost : IRequest
    {
        public Guid Id { get; set; }
    }
}
