using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.Account.Commands;
using MTAA_Backend.Application.Identity.CommandHandlers;
using MTAA_Backend.Domain.Entities.Users;
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

namespace MTAA_Backend.Application.Account.CommandHandlers
{
    public class PresetUpdateAccountAvatarHandler(ILogger<SignUpByEmailHandler> logger,
        IStringLocalizer<ErrorMessages> localizer,
        MTAA_BackendDbContext dbContext,
        IUserService userService) : IRequestHandler<PresetUpdateAccountAvatar>
    {
        private readonly ILogger _logger = logger;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IUserService _userService = userService;

        public async Task Handle(PresetUpdateAccountAvatar request, CancellationToken cancellationToken)
        {
            var imageGroup = await _dbContext.UserPresetAvatarImages.FindAsync(Guid.Parse(request.ImageGroupId));
            if (imageGroup == null)
            {
                _logger.LogError($"Preset Avatar not found: {request.ImageGroupId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.PresetAvatarNotFound], HttpStatusCode.NotFound);
            }

            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authorized");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.NotFound);
            }
            var user = await _dbContext.Users.Where(e => e.Id == userId)
                                             .Include(e => e.Avatar)
                                                .ThenInclude(e => e.PresetAvatar)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (user.Avatar == null)
            {
                var avatar = new UserAvatar()
                {
                    PresetAvatar = imageGroup
                };
                user.Avatar = avatar;
                _dbContext.UserAvatars.Add(avatar);
            }
            else
            {
                user.Avatar.PresetAvatar = imageGroup;
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
