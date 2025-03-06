using MediatR;

namespace MTAA_Backend.Application.CQRS.Posts.Commands
{
    public class AddPost : IRequest<Guid>
    {
        public ICollection<IFormFile> Images { get; set; }
        public string Description { get; set; }
    }
}
