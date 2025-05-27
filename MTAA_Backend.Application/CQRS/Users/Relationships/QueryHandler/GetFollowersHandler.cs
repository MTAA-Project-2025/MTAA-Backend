using AutoMapper;
using MailKit.Search;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Users.Relationships.Queries;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Posts.Embeddings;
using MTAA_Backend.Infrastructure;
using System.Linq.Expressions;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Users.Relationships.QueryHandler
{
    /// <summary>
    /// Handles the <see cref="GetFollowers"/> query to retrieve a paginated and optionally filtered list of users who are following the current user.
    /// It also populates avatar information for each follower.
    /// </summary>
    public class GetFollowersHandler(ILogger<GetFollowersHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IMapper _mapper,
        IUserService _userService) : IRequestHandler<GetFollowers, ICollection<PublicBaseAccountResponse>>
    {
        /// <summary>
        /// Handles the <see cref="GetFollowers"/> query.
        /// </summary>
        /// <param name="request">The <see cref="GetFollowers"/> query request, including pagination and an optional filter string.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of <see cref="PublicBaseAccountResponse"/> representing the followers of the current user.</returns>
        /// <exception cref="HttpException">Thrown if the current user is not authorized.</exception>
        public async Task<ICollection<PublicBaseAccountResponse>> Handle(GetFollowers request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authorized");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.Unauthorized);
            }

            Expression<Func<UserRelationship, bool>> filterCondition = r => (r.User2Id == userId && r.IsUser1Following && !r.IsUser2Following) ||
                                                                            (r.User1Id == userId && r.IsUser2Following && !r.IsUser1Following);

            if (request.FilterStr != null && request.FilterStr != "")
            {
                filterCondition = filterCondition.And(r => (r.User1Id == userId && EF.Functions.Like(r.User2.DisplayName, $"%{request.FilterStr}%")) ||
                                                           (r.User2Id == userId && EF.Functions.Like(r.User1.DisplayName, $"%{request.FilterStr}%")));
            }

            var followers = await _dbContext.UserRelationships
                .Where(filterCondition)
                .Include(e => e.User1.Avatar)
                    .ThenInclude(e => e.CustomAvatar)
                        .ThenInclude(e => e.Images)
                .Include(e => e.User1.Avatar)
                    .ThenInclude(e => e.PresetAvatar)
                        .ThenInclude(e => e.Images)
                .Include(r => r.User2.Avatar)
                    .ThenInclude(e => e.CustomAvatar)
                        .ThenInclude(e => e.Images)
                .Include(e => e.User2.Avatar)
                    .ThenInclude(e => e.PresetAvatar)
                        .ThenInclude(e => e.Images)
                .OrderBy(r => r.User1Id == userId ? r.User2.UserName : r.User1.UserName)
                .Select(r => r.User1Id == userId ? r.User2 : r.User1)
                .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                .Take(request.PageParameters.PageSize)
                .ToListAsync(cancellationToken);

            var mappedFollowers = _mapper.Map<List<PublicBaseAccountResponse>>(followers);

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
            }
            return mappedFollowers;
        }
    }
}
