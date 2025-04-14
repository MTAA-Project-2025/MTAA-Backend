using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Commands;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.BaseGroups.CommandHandlers
{
    public class DeleteGroupHandler(ILogger<DeleteGroupHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IMediator _mediator) : IRequestHandler<DeleteGroup>
    {
        public async Task Handle(DeleteGroup request, CancellationToken cancellationToken)
        {
            var group = await _dbContext.BaseGroups.Where(e => e.Id == request.Id)
                                                   .Include(g => g.Participants)
                                                   .FirstOrDefaultAsync(cancellationToken);
            if (group == null)
            {
                _logger.LogError(ErrorMessagesPatterns.GroupNotFound);
                throw new HttpException(_localizer[ErrorMessagesPatterns.GroupNotFound], HttpStatusCode.NotFound);
            }

            foreach (var participant in group.Participants)
            {
                await _mediator.Send(new LeaveGroup
                {
                    GroupId = group.Id,
                    UserId = participant.Id
                }, cancellationToken);
            }

            group.IsDeleted = true;
            group.DataLastDeleteTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
