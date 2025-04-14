using MediatR;
using MTAA_Backend.Domain.Entities.Posts;

namespace MTAA_Backend.Application.CQRS.Posts.Events
{
    public class GetRecommendedPostsEvent : INotification
    {
        public string UserId { get; set; }
        public List<Post> Posts { get; set; }
    }
}
