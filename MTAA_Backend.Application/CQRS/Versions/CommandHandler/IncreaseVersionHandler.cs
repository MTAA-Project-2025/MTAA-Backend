using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Versions.Command;
using MTAA_Backend.Infrastructure;
using System;

namespace MTAA_Backend.Application.CQRS.Versions.CommandHandler
{
    public class IncreaseVersionHandler(MTAA_BackendDbContext _dbContext) : IRequestHandler<IncreaseVersion>
    {
        public async Task Handle(IncreaseVersion request, CancellationToken cancellationToken)
        {
            var versionItem = await _dbContext.VersionItems.SingleOrDefaultAsync(v => v.UserId == request.UserId && v.Type == request.VersionItemType, cancellationToken);
            if (versionItem != null)
            {
                versionItem.Version++;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
