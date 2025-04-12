using MailKit;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.Entities.Locations;
using MTAA_Backend.Domain.Interfaces.Locations;
using MTAA_Backend.Domain.Resources.Locations;
using MTAA_Backend.Domain.Resources.Other;
using MTAA_Backend.Infrastructure;
using Nest;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace MTAA_Backend.Application.Services.Locations
{
    public class LocationService : ILocationService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly INormalizeLocationService _normalizeLocationService;
        public LocationService(MTAA_BackendDbContext dbContext,
            INormalizeLocationService normalizeLocationService)
        {
            _dbContext = dbContext;
            _normalizeLocationService = normalizeLocationService;
        }
        public async Task AddPoints(double longitude, double latitude, Guid locationId, CancellationToken cancellationToken = default)
        {
            _normalizeLocationService.NormalizeLocation(ref latitude, ref longitude);
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: LocationConstants.SRID);

            for (int i = LocationConstants.MAX_ZOOM_LEVEL; i >= LocationConstants.MIN_ZOOM_LEVEL; i--)
            {
                var (x, y) = LatLonToTile(longitude, latitude, i);
                var envelope = TileToBoundingBox(x, y, i);

                var bbox = geometryFactory.ToGeometry(envelope);

                LocationPoint? oldPoint = null;
                if (i != 0)
                {
                    oldPoint = await _dbContext.LocationPoints
                            .Where(e => e.ZoomLevel == i && !e.IsSubPoint && e.IsVisible && e.Coordinates.Intersects(bbox))
                            .FirstOrDefaultAsync(cancellationToken);
                }
                else
                {
                    oldPoint = await _dbContext.LocationPoints
                            .Where(e => e.ZoomLevel == i && !e.IsSubPoint && e.IsVisible)
                            .FirstOrDefaultAsync(cancellationToken);
                }

                var point = new LocationPoint
                {
                    Coordinates = geometryFactory.CreatePoint(new Coordinate(longitude, latitude)),
                    LocationId = locationId,
                    ZoomLevel = i,
                    Type = LocationPointType.Point,
                };

                if (oldPoint == null)
                {
                    _dbContext.LocationPoints.Add(point);
                }
                else if (oldPoint.Type == LocationPointType.Cluster)
                {
                    point.ParentId = oldPoint.Id;
                    point.IsSubPoint = true;
                    _dbContext.LocationPoints.Add(point);
                }
                else
                {
                    double centerX = envelope.MinX + (envelope.MaxX - envelope.MinX) / 2.0;
                    double centerY = envelope.MinY + (envelope.MaxY - envelope.MinY) / 2.0;

                    point.IsSubPoint = true;
                    oldPoint.IsSubPoint = true;

                    var cluster = new LocationPoint
                    {
                        Coordinates = geometryFactory.CreatePoint(new Coordinate(centerX, centerY)),
                        ZoomLevel = i,
                        Type = LocationPointType.Cluster,
                    };
                    _dbContext.LocationPoints.Add(cluster);
                    point.Parent = cluster;
                    oldPoint.Parent = cluster;
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task DeletePoints(Domain.Entities.Locations.Location location, CancellationToken cancellationToken = default)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: LocationConstants.SRID);
            var oldPoints = await _dbContext.LocationPoints
                .Where(e => e.LocationId == location.Id)
                .Include(e => e.Parent)
                .Select(e => new
                {
                    Point = e,
                    ParentChildCount = e.Parent == null ? 0 : e.Parent.LocationPoints.Where(e => e.IsVisible).Count(),
                })
                .ToListAsync(cancellationToken);

            for (int i = LocationConstants.MAX_ZOOM_LEVEL; i >= LocationConstants.MIN_ZOOM_LEVEL; i--)
            {
                var oldPoint = oldPoints.FirstOrDefault(e => e.Point.ZoomLevel == i);
                if (oldPoint == null) continue;

                _dbContext.LocationPoints.Remove(oldPoint.Point);
                if (oldPoint.Point.Parent == null) continue;
                if (oldPoint.ParentChildCount > 2) continue;
                _dbContext.LocationPoints.Remove(oldPoint.Point.Parent);

                if (oldPoint.ParentChildCount == 2)
                {
                    var childParentPoint = await _dbContext.LocationPoints
                        .Where(e => e.ParentId == oldPoint.Point.Parent.Id && e.IsVisible && e.Id != oldPoint.Point.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (childParentPoint != null)
                    {
                        childParentPoint.IsSubPoint = false;
                    }
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task CorrectLocationsBackgroundJob()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: LocationConstants.SRID);
            DateTime timeNow = DateTime.UtcNow;

            for (int i = LocationConstants.MAX_ZOOM_LEVEL; i >= LocationConstants.MIN_ZOOM_LEVEL; i--)
            {
                var points = await _dbContext.LocationPoints.Where(e => e.IsVisible && e.Location != null && e.Location.EventTime > timeNow)
                                                            .ToListAsync();

                foreach (var point in points)
                {
                    point.IsVisible = false;

                    var (x, y) = LatLonToTile(point.Coordinates.X, point.Coordinates.Y, i);
                    var envelope = TileToBoundingBox(x, y, i);

                    var bbox = geometryFactory.ToGeometry(envelope);

                    var oldPoint = await _dbContext.LocationPoints
                        .Where(e => e.ZoomLevel == i && !e.IsSubPoint && e.Coordinates.Intersects(bbox))
                        .Select(e => new
                        {
                            Point = e,
                            ChildCount = e.LocationPoints.Where(e => e.IsVisible).Count(),
                        })
                        .FirstOrDefaultAsync();

                    if (oldPoint == null || oldPoint.Point == null || oldPoint.ChildCount > 2)
                    {
                        continue;
                    }
                    if (oldPoint.ChildCount == 2)
                    {
                        var childPoint = await _dbContext.LocationPoints
                            .Where(e => e.ParentId == oldPoint.Point.Id && e.IsVisible && e.Id != point.Id)
                            .FirstOrDefaultAsync();

                        if (childPoint != null)
                        {
                            childPoint.IsSubPoint = false;
                        }
                        _dbContext.LocationPoints.Remove(oldPoint.Point);
                    }
                    else
                    {
                        _dbContext.LocationPoints.Remove(oldPoint.Point);
                    }
                }
            }
        }

        //Taken from GPT and modified
        public (int x, int y) LatLonToTile(double longitude, double latitude, int zoomLevel)
        {
            double rLat = Math.PI * latitude / (LocationConstants.MAX_LATITUDE * 2.0);
            int x = (int)Math.Floor((longitude + LocationConstants.MAX_LONGITUDE) / (LocationConstants.MAX_LONGITUDE * 2.0) * (1 << zoomLevel));
            int y = (int)Math.Floor((1.0 - Math.Log(Math.Tan(rLat) + 1 / Math.Cos(rLat)) / Math.PI) / 2.0 * (1 << zoomLevel));
            return (x, y);
        }

        //Taken from GPT and modified
        public NetTopologySuite.Geometries.Envelope TileToBoundingBox(int x, int y, int zoom)
        {
            double n = Math.Pow(2, zoom);

            double lonMin = (double)x / n * (LocationConstants.MAX_LONGITUDE * 2.0) - LocationConstants.MAX_LONGITUDE;
            double lonMax = ((double)x + 1.0) / n * (LocationConstants.MAX_LONGITUDE * 2.0) - LocationConstants.MAX_LONGITUDE;

            double latMinRad = Math.Atan(Math.Sinh(Math.PI * (1.0 - 2.0 * ((double)y + 1) / n)));
            double latMaxRad = Math.Atan(Math.Sinh(Math.PI * (1.0 - 2.0 * (double)y / n)));

            double latMin = latMinRad * LocationConstants.MAX_LONGITUDE / Math.PI;
            double latMax = latMaxRad * LocationConstants.MAX_LONGITUDE / Math.PI;

            double epsilon = 0.00001;
            if (lonMin <= LocationConstants.MIN_LONGITUDE) lonMin += epsilon;
            if (lonMax >= LocationConstants.MAX_LONGITUDE) lonMax -= epsilon;
            if (latMin <= LocationConstants.MIN_LATITUDE) latMin += epsilon;
            if (latMax >= LocationConstants.MAX_LATITUDE) latMax -= epsilon;

            return new NetTopologySuite.Geometries.Envelope(lonMin, lonMax, latMin, latMax);
        }
    }
}
