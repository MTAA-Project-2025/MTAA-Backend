using MediatR;

namespace MTAA_Backend.Application.CQRS.Users.Relationships.Commands
{
    public class Unfollow : IRequest
    {
        public string TargetUserId { get; set; }
    }
}
