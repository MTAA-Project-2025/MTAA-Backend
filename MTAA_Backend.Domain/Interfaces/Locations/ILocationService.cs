using MTAA_Backend.Domain.Entities.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.Locations
{
    public interface ILocationService
    {
        public Task AddPoints(double longitude, double latitude, Guid locationId, CancellationToken cancellationToken = default);
        public Task DeletePoints(Location location, CancellationToken cancellationToken = default);
        public Task CorrectLocationsBackgroundJob();
    }
}
