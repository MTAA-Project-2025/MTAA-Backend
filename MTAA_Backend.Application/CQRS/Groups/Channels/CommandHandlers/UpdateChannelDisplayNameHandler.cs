using MediatR;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Application.CQRS.Users.Identity.Commands;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.CommandHandlers
{
    public class UpdateChannelDisplayNameHandler(MTAA_BackendDbContext _dbContext) : IRequestHandler<UpdateChannelDisplayName>
    {
        public async Task Handle(UpdateChannelDisplayName request, CancellationToken cancellationToken)
        {
            var channel = await _dbContext.Channels.FindAsync(request.Id);
            channel.DisplayName = request.DisplayName;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
