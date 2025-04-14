using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Domain.DTOs.Posts.Requests;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Entities.Posts;

namespace MTAA_Backend.Application.MaperProfiles.Posts
{
    public class PostsMapperProfile : AutoMapper.Profile
    {
        public PostsMapperProfile()
        {
            CreateMap<AddPostRequest, AddPost>();
            CreateMap<UpdatePostRequest, UpdatePost>();
            CreateMap<GlobalSearchRequest, GetGlobalPosts>();
            CreateMap<Post, FullPostResponse>()
                .ForMember(dest => dest.IsLiked, opt => opt.Ignore());
        }
    }
}
