using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.BaseGroups.Events
{
    public class LeaveGroupEvent : INotification
    {
        public Guid GroupId { get; set; }
        public string UserId { get; set; }
    }
}
