using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Queries;
using MTAA_Backend.Application.CQRS.Groups.Messages.Queries;
using MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Queries;
using MTAA_Backend.Domain.DTOs.Groups.UserGroupMemberships.Responses;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.QueryHandlers
{
    public class GetArchivedUserGroupMembershipsHandler(ILogger<GetArchivedUserGroupMembershipsHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IMapper _mapper,
        IMediator _mediator,
        IUserService _userService) : IRequestHandler<GetArchivedUserGroupMemberships, ICollection<SimpleUserGroupMembershipResponse>>
    {
        public async Task<ICollection<SimpleUserGroupMembershipResponse>> Handle(GetArchivedUserGroupMemberships request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authorized");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.BadRequest);
            }

            List<SimpleUserGroupMembershipResponse> mappedMemberships = new List<SimpleUserGroupMembershipResponse>(request.PageParameters.PageSize);

            var memberships = await _dbContext.UserGroupMemberships.Where(e => e.UserId == userId && e.IsArchived)
                                                                   .OrderByDescending(e => e.UnreadMessagesCount)
                                                                   .ThenByDescending(e => e.Group.DataCreationTime)
                                                                   .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                                                                   .Take(request.PageParameters.PageSize)
                                                                   .ToListAsync(cancellationToken);

            foreach (var member in memberships)
            {
                var mappedMembership = _mapper.Map<SimpleUserGroupMembershipResponse>(member);
                mappedMembership.Group = await _mediator.Send(new GetSimpleGroupById()
                {
                    Id = member.GroupId
                });
                if (member.LastMessageId != null)
                {
                    mappedMembership.LastMessage = await _mediator.Send(new GetMessageById()
                    {
                        Id = (Guid)member.LastMessageId
                    });
                }
                mappedMemberships.Add(mappedMembership);
            }
            return mappedMemberships;
        }
    }
}
