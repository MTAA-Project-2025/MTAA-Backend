using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.BaseGroups.Events
{
    public class JoinGroupEvent : INotification
    {
        public Guid GroupId { get; set; }
        public string UserId { get; set; }
    }
}
