using MTAA_Backend.Application.CQRS.Comments.Commands;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Domain.DTOs.Comments.Requests;
using MTAA_Backend.Domain.DTOs.Comments.Responses;
using MTAA_Backend.Domain.DTOs.Posts.Requests;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Posts.Comments;

namespace MTAA_Backend.Application.MaperProfiles.Comments
{
    public class CommentsMapperProfile : AutoMapper.Profile
    {
        public CommentsMapperProfile()
        {
            CreateMap<AddCommentRequest, AddComment>();
            CreateMap<EditCommentRequest, EditComment>();
            CreateMap<Comment, FullCommentResponse>();
        }
    }
}
