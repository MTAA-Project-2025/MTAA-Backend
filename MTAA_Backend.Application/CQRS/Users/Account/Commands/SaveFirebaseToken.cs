using MediatR;

namespace MTAA_Backend.Application.CQRS.Users.Account.Commands
{
    public class SaveFirebaseToken : IRequest
    {
        public String Token { get; set; }
    }
}
