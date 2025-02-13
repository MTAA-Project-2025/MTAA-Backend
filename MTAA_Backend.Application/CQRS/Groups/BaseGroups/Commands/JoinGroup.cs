using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.BaseGroups.Commands
{
    public class JoinGroup : IRequest
    {
        public Guid GroupId { get; set; }
        public string UserId { get; set; }
    }
}
