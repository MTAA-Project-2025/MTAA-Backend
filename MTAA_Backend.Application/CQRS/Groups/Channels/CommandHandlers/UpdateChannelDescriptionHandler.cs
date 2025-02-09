using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.CommandHandlers
{
    public class UpdateChannelDescriptionHandler(MTAA_BackendDbContext dbContext) : IRequestHandler<UpdateChannelDescription>
    {
        private readonly MTAA_BackendDbContext _dbContext = dbContext;

        public async Task Handle(UpdateChannelDescription request, CancellationToken cancellationToken)
        {
            var channel = await _dbContext.Channels.FindAsync(request.Id);
            channel.Description = request.Description;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
