using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Domain.DTOs.Notifications.Responses;
using MTAA_Backend.Domain.DTOs.Posts.Requests;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Entities.Notifications;
using MTAA_Backend.Domain.Entities.Posts;

namespace MTAA_Backend.Application.MaperProfiles.Notifications
{
    public class NotificationMapperProfile : AutoMapper.Profile
    {
        public NotificationMapperProfile()
        {
            CreateMap<Notification, NotificationResponse>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}
