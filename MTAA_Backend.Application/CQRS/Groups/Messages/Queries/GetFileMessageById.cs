using MediatR;
using MTAA_Backend.Domain.DTOs.Messages.Responses;

namespace MTAA_Backend.Application.CQRS.Groups.Messages.Queries
{
    public class GetFileMessageById : IRequest<FileMessageResponse>
    {
        public Guid Id { get; set; }
    }
}
