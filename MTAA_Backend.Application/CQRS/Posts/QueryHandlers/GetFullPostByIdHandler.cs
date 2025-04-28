using AutoMapper;
using MediatR;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.Exceptions;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using System.Net;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Entities.Users;

namespace MTAA_Backend.Application.CQRS.Posts.QueryHandlers
{
    public class GetFullPostByIdHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper,
        IStringLocalizer<ErrorMessages> _localizer,
        ILogger<GetFullPostByIdHandler> _logger) : IRequestHandler<GetFullPostById, FullPostResponse>
    {
        public async Task<FullPostResponse> Handle(GetFullPostById request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var post = await _dbContext.Posts.Where(e => e.Id == request.Id)
                                  .Include(e=>e.Location)
                                  .Include(e => e.Owner)
                                      .ThenInclude(e => e.Avatar)
                                          .ThenInclude(e => e.CustomAvatar)
                                              .ThenInclude(e => e.Images)
                                  .Include(e => e.Owner)
                                      .ThenInclude(e => e.Avatar)
                                          .ThenInclude(e => e.PresetAvatar)
                                              .ThenInclude(e => e.Images)
                                  .Include(e => e.Images)
                                      .ThenInclude(e => e.Images)
                                  .FirstOrDefaultAsync(cancellationToken);

            if (post == null)
            {
                _logger.LogError($"post not found {request.Id}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.PostNotFound], HttpStatusCode.NotFound);
            }

            var postLike = await _dbContext.PostLikes.Where(e => e.UserId == userId && e.PostId==post.Id)
                                                         .FirstOrDefaultAsync(cancellationToken);

            var response = _mapper.Map<FullPostResponse>(post);
            response.IsLiked = postLike != null;

            if (post.Location != null)
            {
                response.LocationId = post.Location.Id;
            }
            if (post.Owner.Avatar != null)
            {
                if (post.Owner.Avatar.CustomAvatar != null)
                {
                    response.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(post.Owner.Avatar.CustomAvatar);
                }
                else if (post.Owner.Avatar.PresetAvatar != null)
                {
                    response.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(post.Owner.Avatar.PresetAvatar);
                }
            }

            return response;
        }
    }
}
