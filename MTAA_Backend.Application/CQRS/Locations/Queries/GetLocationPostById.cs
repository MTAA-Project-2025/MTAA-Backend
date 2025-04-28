using MediatR;
using MTAA_Backend.Domain.DTOs.Posts.Responses;

namespace MTAA_Backend.Application.CQRS.Locations.Queries
{
    public class GetLocationPostById : IRequest<LocationPostResponse>
    {
        public Guid Id { get; set; }
    }
}
