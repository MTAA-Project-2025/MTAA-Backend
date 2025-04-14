using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands
{
    public class ForbidUserGroupMembershipNotifications : IRequest
    {
        public Guid Id { get; set; }
    }
}
