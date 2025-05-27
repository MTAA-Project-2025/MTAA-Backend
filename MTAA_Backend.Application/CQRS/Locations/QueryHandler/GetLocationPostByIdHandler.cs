using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Locations.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Locations.Responses;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Locations.QueryHandler
{
    /// <summary>
    /// Handles the <see cref="GetLocationPostById"/> query to retrieve a specific post associated with a location.
    /// </summary>
    public class GetLocationPostByIdHandler(MTAA_BackendDbContext _dbContext,
        IMapper _mapper,
        ILogger<GetLocationPostByIdHandler> _logger,
        IStringLocalizer<ErrorMessagesPatterns> _localizer,
        IUserService _userService) : IRequestHandler<GetLocationPostById, LocationPostResponse>
    {
        /// <summary>
        /// Handles the <see cref="GetLocationPostById"/> query.
        /// </summary>
        /// <param name="request">The <see cref="GetLocationPostById"/> query request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="LocationPostResponse"/> containing the details of the post and its associated location data.</returns>
        /// <exception cref="HttpException">Thrown if the post is not found or is hidden and the user is not the owner.</exception>
        public async Task<LocationPostResponse> Handle(GetLocationPostById request, CancellationToken cancellationToken)
        {
            string userId = _userService.GetCurrentUserId();
            var post = await _dbContext.Posts
                .Where(p => p.Location!=null && p.Location.Id == request.Id)
                .Include(e => e.Images)
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
                    OwnerDisplayName = p.Owner.DisplayName,
                    Version = p.Version,
                    IsHidden = p.IsHidden,
                    OwnerId = p.OwnerId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (post == null || (post.IsHidden && post.OwnerId != userId))
            {
                _logger.LogError($"post not found {request.Id}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.PostNotFound], HttpStatusCode.NotFound);
            }

            MyImageResponse img = _mapper.Map<MyImageResponse>(post.Image.Images.Where(e => e.Type == ImageSizeType.Small).First());


            if (post.LocationPoint == null)
            {
                return null;
            }

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
            

            return new LocationPostResponse
            {
                Id = post.Id,
                EventTime = post.EventTime,
                DataCreationTime = post.DataCreationTime,
                LocationId = post.LocationId,
                Description = post.Description,
                Point = point,
                SmallFirstImage = img,
                OwnerDisplayName = post.OwnerDisplayName,
                Version = post.Version
            };
        }
    }
}
