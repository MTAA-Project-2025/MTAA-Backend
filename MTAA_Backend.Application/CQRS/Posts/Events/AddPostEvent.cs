using MediatR;

namespace MTAA_Backend.Application.CQRS.Posts.Events
{
    public class AddPostEvent : INotification
    {
        public Guid PostId { get; set; }
        public string UserId { get; set; }
    }
}
