using MediatR;
using MTAA_Backend.Domain.DTOs.Images.Response;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.Commands
{
    public class UpdateChannelImage : IRequest<MyImageGroupResponse>
    {
        public Guid Id { get; set; }
        public IFormFile Image { get; set; }
    }
}
