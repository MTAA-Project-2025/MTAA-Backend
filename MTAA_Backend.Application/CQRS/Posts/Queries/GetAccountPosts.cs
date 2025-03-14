using MediatR;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;

namespace MTAA_Backend.Application.CQRS.Posts.Queries
{
    public class GetAccountPosts : IRequest<ICollection<SimplePostResponse>>
    {
        public string UserId { get; set; }
        public PageParameters PageParameters { get; set; }
    }
}
