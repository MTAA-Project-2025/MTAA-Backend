using MediatR;
using MTAA_Backend.Domain.DTOs.Groups.UserGroupMemberships.Responses;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Queries
{
    public class GetUserGroupMembershipById : IRequest<SimpleUserGroupMembershipResponse>
    {
        public Guid Id { get; set; }
    }
}
