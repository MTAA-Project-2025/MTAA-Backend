using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.Commands
{
    public class UpdateChannelDescription : IRequest
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}
