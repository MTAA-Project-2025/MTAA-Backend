using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.Commands
{
    public class UpdateChannelIdentificationName : IRequest
    {
        public Guid Id { get; set; }
        public string IdentificationName { get; set; }
    }
}
