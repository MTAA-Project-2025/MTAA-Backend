using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands
{
    internal class RemoveUserGroupMembership : IRequest
    {
        public Guid Id { get; set; }
    }
}
