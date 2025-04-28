using MediatR;
using MTAA_Backend.Domain.DTOs.Comments.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;

namespace MTAA_Backend.Application.CQRS.Comments.Queries
{
    public class GetChildComments : IRequest<ICollection<FullCommentResponse>>
    {
        public Guid ParentCommentId { get; set; }
        public PageParameters PageParameters { get; set; }
    }
}
