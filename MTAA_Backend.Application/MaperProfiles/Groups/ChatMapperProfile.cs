using MTAA_Backend.Application.CQRS.Groups.Chats.Commands;
using MTAA_Backend.Domain.DTOs.Groups.Chats.Requests;

namespace MTAA_Backend.Application.MaperProfiles.Groups
{
    public class ChatMapperProfile : AutoMapper.Profile
    {
        public ChatMapperProfile()
        {
            CreateMap<AddChatRequest, AddChat>();
        }
    }
}
