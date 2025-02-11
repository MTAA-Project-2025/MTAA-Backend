using MediatR;
using MTAA_Backend.Domain.Resources.Groups;

namespace MTAA_Backend.Application.CQRS.Groups.Chats.Commands
{
    public class AddChat : IRequest<Guid>
    {
        public string Visibility { get; set; }
        public string IdentificationName { get; set; }
    }
}
