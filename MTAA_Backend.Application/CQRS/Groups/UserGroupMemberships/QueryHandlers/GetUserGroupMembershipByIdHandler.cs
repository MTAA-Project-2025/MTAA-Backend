using AutoMapper;
using MediatR;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Queries;
using MTAA_Backend.Application.CQRS.Groups.Messages.Queries;
using MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Queries;
using MTAA_Backend.Domain.DTOs.Groups.UserGroupMemberships.Responses;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.QueryHandlers
{
    public class GetUserGroupMembershipByIdHandler(MTAA_BackendDbContext dbContext,
        IMapper mapper,
        IMediator mediator) : IRequestHandler<GetUserGroupMembershipById, SimpleUserGroupMembershipResponse>
    {
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;

        public async Task<SimpleUserGroupMembershipResponse> Handle(GetUserGroupMembershipById request, CancellationToken cancellationToken)
        {
            var membership = await _dbContext.UserGroupMemberships.FindAsync(request.Id);

            var mappedMembership = _mapper.Map<SimpleUserGroupMembershipResponse>(membership);
            mappedMembership.Group = await _mediator.Send(new GetSimpleGroupById()
            {
                Id = membership.GroupId
            });
            if (membership.LastMessageId != null)
            {
                mappedMembership.LastMessage = await _mediator.Send(new GetMessageById()
                {
                    Id = (Guid)membership.LastMessageId
                });
            }

            return mappedMembership;
        }
    }
}
