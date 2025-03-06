using MTAA_Backend.Domain.DTOs.Messages.Responses;
using MTAA_Backend.Domain.Entities.Messages;

namespace MTAA_Backend.Application.MaperProfiles.Messages
{
    public class MessagesMapperProfile : AutoMapper.Profile
    {
        public MessagesMapperProfile()
        {
            CreateMap<FileMessage, FileMessageResponse>();
            CreateMap<GifMessage, GifMessageResponse>();
            CreateMap<ImagesMessage, ImagesMessageResponse>();
            CreateMap<TextMessage, TextMessageResponse>();
            CreateMap<VoiceMessage, VoiceMessageResponse>();
        }
    }
}
