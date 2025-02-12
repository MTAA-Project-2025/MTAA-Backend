using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands
{
    public class UnarchiveUserGroupMembership : IRequest
    {
        public Guid Id { get; set; }
    }
}
