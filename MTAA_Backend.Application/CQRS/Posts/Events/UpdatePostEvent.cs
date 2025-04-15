using MediatR;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;

namespace MTAA_Backend.Application.CQRS.Posts.Events
{
    public class UpdatePostEvent : INotification
    {
        public Guid PostId { get; set; }
        public string UserId { get; set; }
    }
}
