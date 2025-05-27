using Azure.Storage.Blobs.Models;
using MediatR;
using MTAA_Backend.Application.CQRS.Locations.Commands;
using MTAA_Backend.Domain.Entities.Locations;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Interfaces.Locations;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Locations.CommandHandlers
{
    /// <summary>
    /// Handles the <see cref="AddPostLocation"/> command to add a new location associated with a post.
    /// </summary>
    public class AddPostLocationHandler(MTAA_BackendDbContext _dbContext,
        ILocationService _locationService) : IRequestHandler<AddPostLocation>
    {
        /// <summary>
        /// Handles the <see cref="AddPostLocation"/> command.
        /// </summary>
        /// <param name="request">The <see cref="AddPostLocation"/> command request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Handle(AddPostLocation request, CancellationToken cancellationToken)
        {
            double latitude = request.Latitude;
            double longitude = request.Longitude;

            Location location = new Location();
            location.PostId = request.PostId;
            location.EventTime = request.EventTime.ToUniversalTime();
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await _locationService.AddPoints(longitude, latitude, location.Id, cancellationToken);
        }
    }
}
