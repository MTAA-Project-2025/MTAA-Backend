using MediatR;

namespace MTAA_Backend.Application.CQRS.Users.Relationships.Commands
{
    public class Follow : IRequest
    {
        public string TargetUserId { get; set; }
    }
}
