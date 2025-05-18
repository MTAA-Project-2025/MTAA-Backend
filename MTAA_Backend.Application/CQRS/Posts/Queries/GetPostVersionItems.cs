using MediatR;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.DTOs.Versioning.Responses;

namespace MTAA_Backend.Application.CQRS.Posts.Queries
{
    public class GetPostVersionItems : IRequest<ICollection<VersionPostItemResponse>>
    {
        public PageParameters PageParameters { get; set; }
    }
}
