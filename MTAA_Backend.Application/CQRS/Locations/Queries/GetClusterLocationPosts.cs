using MediatR;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;

namespace MTAA_Backend.Application.CQRS.Locations.Queries
{
    public class GetClusterLocationPosts : IRequest<ICollection<LocationPostResponse>>
    {
        public Guid CluserPointId { get; set; }
        public PageParameters PageParameters { get; set; }
    }
}
