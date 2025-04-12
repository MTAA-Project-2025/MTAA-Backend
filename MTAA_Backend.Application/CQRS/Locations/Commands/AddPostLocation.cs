using MediatR;

namespace MTAA_Backend.Application.CQRS.Locations.Commands
{
    public class AddPostLocation : IRequest
    {
        public Guid PostId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DateTime EventTime { get; set; }
    }
}
