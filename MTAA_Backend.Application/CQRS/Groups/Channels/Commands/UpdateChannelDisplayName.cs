using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.Commands
{
    public class UpdateChannelDisplayName : IRequest
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
}
