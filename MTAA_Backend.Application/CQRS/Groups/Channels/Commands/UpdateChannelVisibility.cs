using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.Commands
{
    public class UpdateChannelVisibility : IRequest
    {
        public Guid Id { get; set; }
        public string Visibility { get; set; }
    }
}
