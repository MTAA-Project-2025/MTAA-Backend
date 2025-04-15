using MediatR;

namespace MTAA_Backend.Application.CQRS.Posts.Events
{
    public class DeletePostEvent : INotification
    {
        public Guid PostId { get; set; }
        public string UserId { get; set; }
    }
}
