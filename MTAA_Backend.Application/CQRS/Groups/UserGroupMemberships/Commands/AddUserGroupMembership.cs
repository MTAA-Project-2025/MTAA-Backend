using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands
{
    internal class AddUserGroupMembership : IRequest<Guid>
    {
        public string UserId { get; set; }
        public Guid GroupId { get; set; }
    }
}
