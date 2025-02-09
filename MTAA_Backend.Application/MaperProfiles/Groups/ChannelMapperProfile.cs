using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;

namespace MTAA_Backend.Application.MaperProfiles.Groups
{
    public class ChannelMapperProfile : AutoMapper.Profile
    {
        public ChannelMapperProfile()
        {
            CreateMap<AddChannelRequest, AddChannel>();
        }
    }
}
