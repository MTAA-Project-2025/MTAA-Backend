using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Users.Relationships.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Users.Relationships.QueryHandler
{
    public class GetFollowersHandler(ILogger<GetFollowersHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IMapper _mapper,
        IUserService _userService) : IRequestHandler<GetFollowers, ICollection<PublicSimpleAccountResponse>>
    {
        public async Task<ICollection<PublicSimpleAccountResponse>> Handle(GetFollowers request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authorized");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.Unauthorized);
            }

            var followers = await _dbContext.UserRelationships
                .Where(r => (r.User2Id == userId && r.IsUser1Following && !r.IsUser2Following) ||
                            (r.User1Id == userId && r.IsUser2Following && !r.IsUser1Following))
                .OrderBy(r => r.User1Id == userId ? r.User2 : r.User1)
                .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                .Take(request.PageParameters.PageSize)
                .Select(r => r.User1Id == userId ? r.User2 : r.User1)
                .Include(e => e.Avatar)
                    .ThenInclude(e => e.CustomAvatar)
                        .ThenInclude(e => e.Images)
                .Include(e => e.Avatar)
                    .ThenInclude(e => e.PresetAvatar)
                        .ThenInclude(e => e.Images)
                .ToListAsync(cancellationToken);

            var mappedFollowers = _mapper.Map<List<PublicSimpleAccountResponse>>(followers);

            for (int i = 0; i < mappedFollowers.Count; i++)
            {
                var follower = followers[i];

                if (follower.Avatar != null)
                {
                    if (follower.Avatar.CustomAvatar != null)
                    {
                        mappedFollowers[i].Avatar = _mapper.Map<MyImageGroupResponse>(follower.Avatar.CustomAvatar);
                    }
                    else if (follower.Avatar.PresetAvatar != null)
                    {
                        mappedFollowers[i].Avatar = _mapper.Map<MyImageGroupResponse>(follower.Avatar.PresetAvatar);
                    }
                }
                mappedFollowers[i].IsFollowed = false;
            }
            return mappedFollowers;
        }
    }
}
