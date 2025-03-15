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
    public class GetFriendsHandler(ILogger<GetFriendsHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IMapper _mapper,
        IUserService _userService) : IRequestHandler<GetFriends, ICollection<PublicSimpleAccountResponse>>
    {
        public async Task<ICollection<PublicSimpleAccountResponse>> Handle(GetFriends request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authorized");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.Unauthorized);
            }

            var friends = await _dbContext.UserRelationships
                .Where(r => ((r.User1Id == userId && r.IsUser1Following && r.IsUser2Following) ||
                             (r.User2Id == userId && r.IsUser1Following && r.IsUser2Following)))
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

            var mappedFriends = _mapper.Map<List<PublicSimpleAccountResponse>>(friends);

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
                mappedFriends[i].IsFollowed = true;
            }
            return mappedFriends;
        }
    }
}
