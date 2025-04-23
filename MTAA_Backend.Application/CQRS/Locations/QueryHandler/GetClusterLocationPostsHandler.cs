using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Locations.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Locations.Responses;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Other;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Locations.QueryHandler
{
    public class GetClusterLocationPostsHandler(MTAA_BackendDbContext _dbContext,
        IMapper _mapper) : IRequestHandler<GetClusterLocationPosts, ICollection<LocationPostResponse>>
    {
        public async Task<ICollection<LocationPostResponse>> Handle(GetClusterLocationPosts request, CancellationToken cancellationToken)
        {
            var posts = await _dbContext.Posts
                .Where(p => p.Location != null && p.Location.Points.Any(lp => lp.ParentId == request.CluserPointId))
                .Include(e=>e.Images)
                    .ThenInclude(e => e.Images)
                .Select(p => new
                {
                    Id = p.Id,
                    EventTime = p.Location.EventTime,
                    DataCreationTime = p.DataCreationTime,
                    LocationId = p.Location.Id,
                    Description = p.Description,
                    LocationPoint = p.Location.Points.First(),
                    Image = p.Images.First(),
                    OwnerDisplayName = p.Owner.DisplayName
                })
                .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                .Take(request.PageParameters.PageSize)
                .ToListAsync(cancellationToken);

            var res = new List<LocationPostResponse>(posts.Count);

            foreach (var post in posts)
            {
                MyImageResponse img = _mapper.Map<MyImageResponse>(post.Image.Images.Where(e => e.Type == ImageSizeType.Small).First());

                var point = new SimpleLocationPointResponse
                {
                    Id = post.LocationPoint.Id,
                    Longitude = post.LocationPoint.Coordinates.X,
                    Latitude = post.LocationPoint.Coordinates.Y,
                    ZoomLevel = post.LocationPoint.ZoomLevel,
                    ChildCount = 0,
                    Image = img,
                    Type = post.LocationPoint.Type,
                    PostId = post.Id,
                };

                res.Add(new LocationPostResponse
                {
                    Id = post.Id,
                    EventTime = post.EventTime,
                    DataCreationTime = post.DataCreationTime,
                    LocationId = post.LocationId,
                    Description = post.Description,
                    Point = point,
                    SmallFirstImage = img,
                    OwnerDisplayName=post.OwnerDisplayName
                });
            }

            return res;
        }
    }
}
