using MediatR;
using MTAA_Backend.Domain.DTOs.Groups.BaseGroups.Responses;

namespace MTAA_Backend.Application.CQRS.Groups.BaseGroups.Queries
{
    public class GetSimpleGroupById : IRequest<SimpleBaseGroupResponse>
    {
        public Guid Id { get; set; }
    }
}
