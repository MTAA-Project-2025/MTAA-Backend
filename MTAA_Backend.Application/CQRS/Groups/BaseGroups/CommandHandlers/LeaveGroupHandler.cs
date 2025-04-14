using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Commands;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Events;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.BaseGroups.CommandHandlers
{
    public class LeaveGroupHandler(ILogger<LeaveGroupHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IMediator _mediator) : IRequestHandler<LeaveGroup>
    {
        public async Task Handle(LeaveGroup request, CancellationToken cancellationToken)
        {
            var group = await _dbContext.BaseGroups.Where(e => e.Id == request.GroupId)
                                                   .Include(e => e.Participants)
                                                   .FirstOrDefaultAsync(cancellationToken);

            if (group == null)
            {
                _logger.LogError($"Group not found {request.GroupId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.GroupNotFound], HttpStatusCode.NotFound);
            }

            var user = group.Participants.FirstOrDefault(e => e.Id == request.UserId);
            if (user == null)
            {
                _logger.LogError($"User is not in the group {request.UserId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotInGroup], HttpStatusCode.NotFound);
            }

            group.Participants.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new LeaveGroupEvent()
            {
                GroupId = group.Id,
                UserId = request.UserId
            }, cancellationToken);
        }
    }
}
