using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Users.Relationships.Queries;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Users.Relationships.QueryHandler
{
    public class GetFriendsHandler(ILogger<GetFriendsHandler> logger,
        IStringLocalizer<ErrorMessages> localizer,
        MTAA_BackendDbContext dbContext,
        IMapper mapper,
        IUserService userService) : IRequestHandler<GetFriends, ICollection<PublicSimpleAccountResponse>>
    {
        private readonly ILogger _logger = logger;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        private readonly IUserService _userService = userService;

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

            return _mapper.Map<ICollection<PublicSimpleAccountResponse>>(friends);
        }
    }
}
