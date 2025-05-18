using MediatR;

namespace MTAA_Backend.Application.CQRS.Posts.Commands
{
    public class UnhidePost : IRequest
    {
        public Guid Id { get; set; }
    }
}
