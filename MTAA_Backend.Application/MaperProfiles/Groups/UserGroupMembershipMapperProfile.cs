using MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands;
using MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Queries;
using MTAA_Backend.Domain.DTOs.Groups.UserGroupMemberships.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Entities.Groups;

namespace MTAA_Backend.Application.MaperProfiles.Groups
{
    public class UserGroupMembershipMapperProfile : AutoMapper.Profile
    {
        public UserGroupMembershipMapperProfile()
        {
            CreateMap<GenericIdRequest<Guid>, AllowUserGroupMembershipNotifications>();
            CreateMap<GenericIdRequest<Guid>, ForbidUserGroupMembershipNotifications>();

            CreateMap<GenericIdRequest<Guid>, ArchiveUserGroupMembership>();
            CreateMap<GenericIdRequest<Guid>, UnarchiveUserGroupMembership>();

            CreateMap<UserGroupMembership, SimpleUserGroupMembershipResponse>()
                .ForMember(ost => ost.LastMessage,
                           dest => dest.Ignore());
        }
    }
}
