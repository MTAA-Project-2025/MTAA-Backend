using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Commands;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Events;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;
using System.Runtime.InteropServices;

namespace MTAA_Backend.Application.CQRS.Groups.BaseGroups.CommandHandlers
{
    public class JoinGroupHandler(ILogger<JoinGroupHandler> logger,
        IStringLocalizer<ErrorMessages> localizer,
        MTAA_BackendDbContext dbContext,
        IMediator mediator) : IRequestHandler<JoinGroup>
    {
        private readonly ILogger _logger = logger;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IMediator _mediator = mediator;

        public async Task Handle(JoinGroup request, CancellationToken cancellationToken)
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
            if (user != null)
            {
                _logger.LogError($"User is already in the group {request.UserId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserAlreadyInGroup], HttpStatusCode.NotFound);
            }

            var newUser = await _dbContext.Users.Where(e => e.Id == request.UserId)
                                                .Include(e => e.Groups)
                                                .FirstOrDefaultAsync(cancellationToken);
            if (newUser == null)
            {
                _logger.LogError($"User not found {request.UserId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound], HttpStatusCode.NotFound);
            }

            group.Participants.Add(newUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new JoinGroupEvent()
            {
                GroupId = group.Id,
                UserId = request.UserId
            }, cancellationToken);
        }
    }
}
