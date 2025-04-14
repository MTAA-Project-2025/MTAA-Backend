using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.CommandHandlers
{
    public class UpdateChannelIdentificationNameHandler(IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext) : IRequestHandler<UpdateChannelIdentificationName>
    {
        public async Task Handle(UpdateChannelIdentificationName request, CancellationToken cancellationToken)
        {
            var channel = await _dbContext.Channels.FindAsync(request.Id);

            var oldchannel = await _dbContext.Channels.Where(e => e.IdentificationName == request.IdentificationName)
                                                      .FirstOrDefaultAsync(cancellationToken);

            if (oldchannel != null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.ChannelAlreadyExist], HttpStatusCode.BadRequest);
            }
            channel.IdentificationName = request.IdentificationName;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
