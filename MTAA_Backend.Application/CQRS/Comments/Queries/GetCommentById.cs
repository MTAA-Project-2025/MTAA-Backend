using MediatR;
using MTAA_Backend.Domain.DTOs.Comments.Responses;

namespace MTAA_Backend.Application.CQRS.Comments.Queries
{
    public class GetCommentById:IRequest<FullCommentResponse>
    {
        public Guid Id { get; set; }
    }
}
