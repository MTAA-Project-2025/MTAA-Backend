using MediatR;

namespace MTAA_Backend.Application.CQRS.Locations.Commands
{
    public class AddPostLocation : IRequest
    {
        public Guid PostId { get; set; }
    }
}
