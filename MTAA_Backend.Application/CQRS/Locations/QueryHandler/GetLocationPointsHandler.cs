using MediatR;
using MTAA_Backend.Application.CQRS.Locations.Queries;
using MTAA_Backend.Domain.DTOs.Locations.Responses;
using MTAA_Backend.Domain.Interfaces.Locations;
using MTAA_Backend.Infrastructure;
using NetTopologySuite.Geometries;
using NetTopologySuite;
using MTAA_Backend.Domain.Resources.Locations;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.Entities.Locations;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Other;
using MTAA_Backend.Domain.DTOs.Images.Response;
using AutoMapper;

namespace MTAA_Backend.Application.CQRS.Locations.QueryHandler
{
    public class GetLocationPointsHandler(MTAA_BackendDbContext _dbContext,
        INormalizeLocationService _normalizeLocationService,
        IMapper _mapper) : IRequestHandler<GetLocationPoints, ICollection<SimpleLocationPointResponse>>
    {
        public async Task<ICollection<SimpleLocationPointResponse>> Handle(GetLocationPoints request, CancellationToken cancellationToken)
        {
            double latitude = request.Latitude;
            double longitude = request.Longitude;
            double radius = request.Radius;
            int zoomLevel = request.ZoomLevel;

            _normalizeLocationService.NormalizeLocation(ref latitude, ref longitude, ref radius, zoomLevel);

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: LocationConstants.SRID);
            var point = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));

            var locations = await _dbContext.LocationPoints.Where(e => e.ZoomLevel == zoomLevel && e.IsVisible && !e.IsSubPoint && e.Coordinates.IsWithinDistance(point, radius))
                                            .Include(e => e.Location)
                                                .ThenInclude(e => e.Post)
                                                    .ThenInclude(e => e.Images)
                                                        .ThenInclude(e => e.Images)
                                            .Select(e => new
                                            {
                                                Point = e,
                                                Image = e.Location.Post.Images.Where(p => p.Position == 0).FirstOrDefault(),
                                                PostId = e.Location == null ? null : e.Location.PostId,
                                                ChildCount = e.LocationPoints.Count(p => p.IsVisible)
                                            })
                                            .ToListAsync(cancellationToken);

            var res = new List<SimpleLocationPointResponse>(locations.Count);

            foreach (var location in locations)
            {
                MyImageResponse img = null;
                if (location.Point.Type == LocationPointType.Point)
                {
                    var origImg = location.Image.Images.Where(e => e.Type == ImageSizeType.Small).FirstOrDefault();
                    if (origImg != null)
                    {
                        img = _mapper.Map<MyImageResponse>(origImg);
                    }
                }

                res.Add(new SimpleLocationPointResponse
                {
                    Id = location.Point.Id,
                    Longitude = location.Point.Coordinates.X,
                    Latitude = location.Point.Coordinates.Y,
                    ZoomLevel = location.Point.ZoomLevel,
                    ChildCount = location.ChildCount,
                    Image = img,
                    Type = location.Point.Type,
                    PostId = location.PostId,
                });
            }

            return res;
        }
    }
}
