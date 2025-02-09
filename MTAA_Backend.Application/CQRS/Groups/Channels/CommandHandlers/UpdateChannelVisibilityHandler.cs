using MediatR;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Resources.Groups;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.CommandHandlers
{
    public class UpdateChannelVisibilityHandler(ILogger<SignUpByEmailHandler> logger,
        IStringLocalizer<ErrorMessages> localizer,
        MTAA_BackendDbContext dbContext) : IRequestHandler<UpdateChannelVisibility>
    {
        private readonly ILogger _logger = logger;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly MTAA_BackendDbContext _dbContext = dbContext;

        public async Task Handle(UpdateChannelVisibility request, CancellationToken cancellationToken)
        {
            var visibilities = GroupVisibilityTypes.GetAllPublic();
            if (!visibilities.Contains(request.Visibility))
            {
                _logger.LogError($"Visibility not found: {request.Visibility}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.GroupVisibilityTypeDontExist], HttpStatusCode.BadRequest);
            }
            var channel = await _dbContext.Channels.FindAsync(request.Id);
            channel.Visibility = request.Visibility;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
