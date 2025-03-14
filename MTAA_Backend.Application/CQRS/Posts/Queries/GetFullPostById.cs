using MediatR;
using MTAA_Backend.Domain.DTOs.Posts.Responses;

namespace MTAA_Backend.Application.CQRS.Posts.Queries
{
    public class GetFullPostById : IRequest<FullPostResponse>
    {
        public Guid Id { get; set; }
    }
}
