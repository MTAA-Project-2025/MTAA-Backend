using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands
{
    public class ArchiveUserGroupMembership : IRequest
    {
        public Guid Id { get; set; }
    }
}
