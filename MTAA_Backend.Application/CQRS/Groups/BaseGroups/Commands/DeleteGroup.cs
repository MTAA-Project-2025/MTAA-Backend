using MediatR;

namespace MTAA_Backend.Application.CQRS.Groups.BaseGroups.Commands
{
    public class DeleteGroup : IRequest
    {
        public Guid Id { get; set; }
    }
}
