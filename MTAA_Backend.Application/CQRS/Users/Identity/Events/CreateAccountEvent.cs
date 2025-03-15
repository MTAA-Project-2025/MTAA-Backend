using MediatR;

namespace MTAA_Backend.Application.CQRS.Users.Identity.Events
{
    public class CreateAccountEvent : INotification
    {
        public string UserId { get; set; }
    }
}
