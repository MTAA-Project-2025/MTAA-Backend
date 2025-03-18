using MediatR;
using MTAA_Backend.Domain.Entities.Versions;

namespace MTAA_Backend.Application.CQRS.Versions.Events
{
    public class AccountUpdatedEvent : INotification
    {
        public string UserId { get; set; }
        public VersionItemType VersionItemType { get; set; }
    }
}
