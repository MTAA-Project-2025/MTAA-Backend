using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Locations.Commands;
using MTAA_Backend.Domain.Interfaces.Locations;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Locations.CommandHandlers
{
    public class UpdatePostLocationHandler(MTAA_BackendDbContext _dbContext,
        ILocationService _locationService,
        IMediator _mediator) : IRequestHandler<UpdatePostLocation>
    {
        public async Task Handle(UpdatePostLocation request, CancellationToken cancellationToken)
        {
            var location = await _dbContext.Locations
                .Where(e => e.PostId == request.PostId)
                .Select(e => new
                {
                    Location = e,
                    Point = e.Points.FirstOrDefault()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (location == null || location.Location == null)
            {
                await _mediator.Send(new AddPostLocation
                {
                    PostId = request.PostId,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    EventTime = request.EventTime
                }, cancellationToken);
                return;
            }
            location.Location.EventTime = request.EventTime.ToUniversalTime();
            await _dbContext.SaveChangesAsync(cancellationToken);

            if (location.Point != null && Math.Abs(location.Point.Coordinates.X - request.Longitude) <= 0.000001 &&
               Math.Abs(location.Point.Coordinates.Y - request.Latitude) <= 0.000001) return;

            await _locationService.DeletePoints(location.Location, cancellationToken);
            await _locationService.AddPoints(request.Longitude, request.Latitude, location.Location.Id, cancellationToken);
        }
    }
}
