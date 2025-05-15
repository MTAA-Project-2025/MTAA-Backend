using MediatR;

namespace MTAA_Backend.Application.CQRS.Posts.Commands
{
    public class HidePost : IRequest
    {
        public Guid Id { get; set; }
        public string? Reason { get; set; }
    }
}
