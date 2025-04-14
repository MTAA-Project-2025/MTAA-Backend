using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.CQRS.Users.Account.QueryHandlers
{
    public class PublicGetFullAccountHandler(ILogger<PublicGetFullAccountHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper) : IRequestHandler<PublicGetFullAccount, PublicFullAccountResponse>
    {
        public async Task<PublicFullAccountResponse> Handle(PublicGetFullAccount request, CancellationToken cancellationToken)
        {
            var customerId = _userService.GetCurrentUserId();

            var user = await _dbContext.Users.Where(e => e.Id == request.UserId)
                                             .Include(e => e.Avatar)
                                                 .ThenInclude(e => e.CustomAvatar)
                                                    .ThenInclude(e => e.Images)
                                             .Include(e => e.Avatar)
                                                 .ThenInclude(e => e.PresetAvatar)
                                                    .ThenInclude(e => e.Images)
                                             .Select(user => new
                                             {
                                                 User = user,
                                                 IsFollowing = user.UserRelationships1.Any(e => e.User2Id == customerId && e.IsUser1Following) ||
                                                               user.UserRelationships2.Any(e => e.User1Id == customerId && e.IsUser2Following),
                                                 FriendsCount = user.UserRelationships1.Count(e => e.IsUser1Following && e.IsUser2Following) +
                                                                user.UserRelationships2.Count(e => e.IsUser1Following && e.IsUser2Following),
                                                 FollowersCount = user.UserRelationships1.Count(e=>!e.IsUser1Following && e.IsUser2Following) +
                                                                  user.UserRelationships2.Count(e => !e.IsUser2Following && e.IsUser1Following)
                                             })
                                             .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                _logger.LogError($"User not found {request.UserId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound], HttpStatusCode.NotFound);
            }

            var response = _mapper.Map<PublicFullAccountResponse>(user.User);
            response.IsFollowing = user.IsFollowing;
            response.FriendsCount = user.FriendsCount;
            response.FollowersCount = user.FollowersCount;

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
