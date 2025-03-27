using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Users.Account.QueryHandlers
{
    public class GetUserFullAccountHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper) : IRequestHandler<GetUserFullAccount, UserFullAccountResponse>
    {
        public async Task<UserFullAccountResponse> Handle(GetUserFullAccount request, CancellationToken cancellationToken)
        {
            var customerId = _userService.GetCurrentUserId();

            var user = await _dbContext.Users.Where(e => e.Id == customerId)
                                             .Include(e => e.Avatar)
                                                 .ThenInclude(e => e.CustomAvatar)
                                                    .ThenInclude(e => e.Images)
                                             .Include(e => e.Avatar)
                                                 .ThenInclude(e => e.PresetAvatar)
                                                    .ThenInclude(e => e.Images)
                                             .Select(user => new
                                             {
                                                 User = user,
                                                 FriendsCount = user.UserRelationships1.Count(e => e.IsUser1Following && e.IsUser2Following) +
                                                                user.UserRelationships2.Count(e => e.IsUser1Following && e.IsUser2Following),
                                                 FollowersCount = user.UserRelationships1.Count(e => !e.IsUser1Following && e.IsUser2Following) +
                                                                  user.UserRelationships2.Count(e => !e.IsUser2Following && e.IsUser1Following),
                                                 LikesCount = user.CreatedPosts.Sum(e => e.LikesCount)
                                             })
                                             .FirstOrDefaultAsync(cancellationToken);

            var response = _mapper.Map<UserFullAccountResponse>(user.User);
            response.IsFollowing = true;
            response.FriendsCount = user.FriendsCount;
            response.FollowersCount = user.FollowersCount;
            response.LikesCount = user.LikesCount;

            if (user.User.Avatar != null)
            {
                if (user.User.Avatar.CustomAvatar != null)
                {
                    response.Avatar = _mapper.Map<MyImageGroupResponse>(user.User.Avatar.CustomAvatar);
                }
                else if (user.User.Avatar.PresetAvatar != null)
                {
                    response.Avatar = _mapper.Map<MyImageGroupResponse>(user.User.Avatar.PresetAvatar);
                }
            }

            return response;
        }
    }
}
