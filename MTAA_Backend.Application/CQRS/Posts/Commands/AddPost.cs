using MediatR;
using MTAA_Backend.Domain.DTOs.Images.Requests;

namespace MTAA_Backend.Application.CQRS.Posts.Commands
{
    public class AddPost : IRequest<Guid>
    {
        public ICollection<AddImageRequest> Images { get; set; }
        public string Description { get; set; }

        public DateTime? SchedulePublishDate { get; set; }
    }
}
