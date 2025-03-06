using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Domain.DTOs.Posts.Requests;

namespace MTAA_Backend.Application.MaperProfiles.Posts
{
    public class PostsMapperProfile : AutoMapper.Profile
    {
        public PostsMapperProfile()
        {
            CreateMap<AddPostRequest, AddPost>();
        }
    }
}
