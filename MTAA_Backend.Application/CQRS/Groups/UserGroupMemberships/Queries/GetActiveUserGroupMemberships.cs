using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using MTAA_Backend.Domain.DTOs.Groups.UserGroupMemberships.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Queries
{
    public class GetActiveUserGroupMemberships : IRequest<ICollection<SimpleUserGroupMembershipResponse>>
    {
        public PageParameters PageParameters { get; set; }
    }
}
