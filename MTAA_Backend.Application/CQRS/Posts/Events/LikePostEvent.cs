using MediatR;

namespace MTAA_Backend.Application.CQRS.Posts.Events
{
    public class LikePostEvent : INotification
    {
        public Guid PostLikeId { get; set; }
        public Guid UserId { get; set; }
    }
}
