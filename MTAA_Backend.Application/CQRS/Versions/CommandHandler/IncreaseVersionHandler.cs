using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Versions.Command;
using MTAA_Backend.Application.Services.Notifications;
using MTAA_Backend.Infrastructure;
using System;

namespace MTAA_Backend.Application.CQRS.Versions.CommandHandler
{
    /// <summary>
    /// Handles the <see cref="IncreaseVersion"/> command, which increments the version number for a specific user's data item
    /// and notifies connected clients via Server-Sent Events (SSE) about the change.
    /// </summary>
    public class IncreaseVersionHandler(MTAA_BackendDbContext _dbContext,
        ISSEClientStorage _sseClientStorage) : IRequestHandler<IncreaseVersion>
    {
        /// <summary>
        /// Handles the <see cref="IncreaseVersion"/> command.
        /// </summary>
        /// <param name="request">The <see cref="IncreaseVersion"/> command request, specifying the user ID and the type of version item to increase.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Handle(IncreaseVersion request, CancellationToken cancellationToken)
        {
            var versionItem = await _dbContext.VersionItems.SingleOrDefaultAsync(v => v.UserId == request.UserId && v.Type == request.VersionItemType, cancellationToken);
            if (versionItem != null)
            {
                versionItem.Version++;
                await _dbContext.SaveChangesAsync(cancellationToken);
                await _sseClientStorage.ChangeVersionAsync(request.UserId, new Domain.DTOs.Versioning.Responses.VersionItemResponse()
                {
                    Type = versionItem.Type,
                    Version = versionItem.Version
                });
            }
        }
    }
}
