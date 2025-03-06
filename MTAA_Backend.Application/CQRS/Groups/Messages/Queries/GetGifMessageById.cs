using MediatR;
using MTAA_Backend.Domain.DTOs.Messages.Responses;

namespace MTAA_Backend.Application.CQRS.Groups.Messages.Queries
{
    public class GetGifMessageById : IRequest<GifMessageResponse>
    {
        public Guid Id { get; set; }
    }
}
