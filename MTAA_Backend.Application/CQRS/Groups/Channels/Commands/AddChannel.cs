using MediatR;
using MTAA_Backend.Domain.Resources.Groups;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.Commands
{
    public class AddChannel : IRequest<Guid>
    {
        public IFormFile? Image { get; set; }
        public string Visibility { get; set; }
        public string DisplayName { get; set; }
        public string IdentificationName { get; set; }
        public string Description { get; set; }
    }
}
