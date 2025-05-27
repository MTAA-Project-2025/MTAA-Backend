using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Application.CQRS.Users.Relationships.Queries;
using MTAA_Backend.Application.CQRS.Users.Relationships.QueryHandler;
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

namespace MTAA_Backend.Application.CQRS.Users.Account.QueryHandlers
{
    /// <summary>
    /// Handles the <see cref="GetGlobalUsers"/> query to retrieve a paginated and optionally filtered list of global users.
    /// It also determines if the current user is following each retrieved user.
    /// </summary>
    public class GetGlobalUsersHandler(ILogger<GetGlobalUsersHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IMapper _mapper,
        IUserService _userService) : IRequestHandler<GetGlobalUsers, ICollection<PublicBaseAccountResponse>>
    {
        /// <summary>
        /// Handles the <see cref="GetGlobalUsers"/> query.
        /// </summary>
        /// <param name="request">The <see cref="GetGlobalUsers"/> query request, including optional filter string and pagination parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of <see cref="PublicBaseAccountResponse"/> containing public account details for users.</returns>
        public async Task<ICollection<PublicBaseAccountResponse>> Handle(GetGlobalUsers request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            Expression<Func<User, bool>> filterCondition = r => !r.IsDeleted;

            if (request.FilterStr != null && request.FilterStr != "")
            {
                filterCondition = filterCondition.And(e => EF.Functions.Like(e.DisplayName, $"%{request.FilterStr}%") || EF.Functions.Like(e.UserName, $"%{request.FilterStr}%"));
            }

            var users = await _dbContext.Users
                .Where(filterCondition)
                .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                .Take(request.PageParameters.PageSize)
                .Include(e => e.Avatar)
                    .ThenInclude(e => e.CustomAvatar)
                        .ThenInclude(e => e.Images)
                .Include(e => e.Avatar)
                    .ThenInclude(e => e.PresetAvatar)
                        .ThenInclude(e => e.Images)
                .Select(user => new
                {
                    User = user,
                    IsFollowing = user.UserRelationships1.Any(e => e.User2Id == userId && e.IsUser2Following) ||
                                  user.UserRelationships2.Any(e => e.User1Id == userId && e.IsUser1Following),
                })
                .ToListAsync(cancellationToken);

            var mappedUsers = _mapper.Map<List<PublicBaseAccountResponse>>(users.Select(e => e.User).ToList());

            for (int i = 0; i < mappedUsers.Count; i++)
            {
                var user = users[i].User;
                mappedUsers[i].IsFollowing = users[i].IsFollowing;

                if (user.Avatar != null)
                {
                    if (user.Avatar.CustomAvatar != null)
                    {
                        mappedUsers[i].Avatar = _mapper.Map<MyImageGroupResponse>(user.Avatar.CustomAvatar);
                    }
                    else if (user.Avatar.PresetAvatar != null)
                    {
                        mappedUsers[i].Avatar = _mapper.Map<MyImageGroupResponse>(user.Avatar.PresetAvatar);
                    }
                }
            }
            return mappedUsers;
        }
    }
}
