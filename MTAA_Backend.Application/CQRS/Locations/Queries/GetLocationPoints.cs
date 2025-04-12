using MediatR;
using MTAA_Backend.Domain.DTOs.Locations.Responses;

namespace MTAA_Backend.Application.CQRS.Locations.Queries
{
    public class GetLocationPoints : IRequest<ICollection<SimpleLocationPointResponse>>
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int ZoomLevel { get; set; }
        public double Radius { get; set; }
    }
}
