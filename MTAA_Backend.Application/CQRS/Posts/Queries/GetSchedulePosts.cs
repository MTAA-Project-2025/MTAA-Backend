using MediatR;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;

namespace MTAA_Backend.Application.CQRS.Posts.Queries
{
    public class GetSchedulePosts : IRequest<ICollection<SchedulePostResponse>>
    {
        public PageParameters PageParameters { get; set; }
    }
}
