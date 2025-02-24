using MTAA_Backend.Domain.DTOs.Groups.BaseGroups.Responses;
using MTAA_Backend.Domain.Entities.Groups;

namespace MTAA_Backend.Application.MaperProfiles.Groups
{
    public class BaseGroupMapperProfile : AutoMapper.Profile
    {
        public BaseGroupMapperProfile()
        {
            CreateMap<BaseGroup, SimpleBaseGroupResponse>()
                .ForMember(ost => ost.Image,
                           dest => dest.Ignore())
                .ForMember(ost => ost.Title,
                           dest => dest.Ignore());
        }
    }
}
