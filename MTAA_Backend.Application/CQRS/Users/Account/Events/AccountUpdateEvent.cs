using MediatR;

namespace MTAA_Backend.Application.CQRS.Users.Account.Events
{
    public class AccountUpdateEvent : INotification
    {
        public string UserId { get; set; }
    }
}
