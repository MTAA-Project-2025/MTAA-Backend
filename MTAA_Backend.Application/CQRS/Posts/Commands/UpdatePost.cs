using MediatR;
using MTAA_Backend.Domain.DTOs.Images.Requests;

namespace MTAA_Backend.Application.CQRS.Posts.Commands
{
    public class UpdatePost : IRequest
    {
        public Guid Id { get; set; }
        public ICollection<UpdateImageRequest> Images { get; set; }
        public string Description { get; set; }
    }
}
