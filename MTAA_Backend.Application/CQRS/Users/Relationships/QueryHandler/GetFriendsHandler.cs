using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Users.Relationships.Queries;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Linq.Expressions;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Users.Relationships.QueryHandler
{
    public class GetFriendsHandler(ILogger<GetFriendsHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IMapper _mapper,
        IUserService _userService) : IRequestHandler<GetFriends, ICollection<PublicBaseAccountResponse>>
    {
        public async Task<ICollection<PublicBaseAccountResponse>> Handle(GetFriends request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authorized");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.Unauthorized);
            }

            Expression<Func<UserRelationship, bool>> filterCondition = r => (r.User2Id == userId && r.IsUser1Following && r.IsUser2Following) ||
                                                                (r.User1Id == userId && r.IsUser2Following && r.IsUser1Following);

            if (request.FilterStr != null && request.FilterStr != "")
            {
                filterCondition = filterCondition.And(r => (r.User1Id == userId && EF.Functions.Like(r.User2.DisplayName, $"%{request.FilterStr}%")) ||
                                                           (r.User2Id == userId && EF.Functions.Like(r.User1.DisplayName, $"%{request.FilterStr}%")));
            }

            var friends = await _dbContext.UserRelationships
                .Where(filterCondition)
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

            var mappedFriends = _mapper.Map<List<PublicBaseAccountResponse>>(friends);

            for (int i = 0; i < mappedFriends.Count; i++)
            {
                var friend = friends[i];

                if (friend.Avatar != null)
                {
                    if (friend.Avatar.CustomAvatar != null)
                    {
                        mappedFriends[i].Avatar = _mapper.Map<MyImageGroupResponse>(friend.Avatar.CustomAvatar);
                    }
                    else if (friend.Avatar.PresetAvatar != null)
                    {
                        mappedFriends[i].Avatar = _mapper.Map<MyImageGroupResponse>(friend.Avatar.PresetAvatar);
                    }
                }
                mappedFriends[i].IsFollowing = true;
            }
            return mappedFriends;
        }
    }
}
