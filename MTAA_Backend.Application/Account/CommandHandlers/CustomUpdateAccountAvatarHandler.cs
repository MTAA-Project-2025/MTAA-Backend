using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.Account.Commands;
using MTAA_Backend.Application.Identity.CommandHandlers;
using MTAA_Backend.Application.Services;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Images;
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
    public class CustomUpdateAccountAvatarHandler(ILogger<SignUpByEmailHandler> logger,
        IStringLocalizer<ErrorMessages> localizer,
        MTAA_BackendDbContext dbContext,
        IUserService userService,
        IImageService imageService) : IRequestHandler<CustomUpdateAccountAvatar>
    {
        private readonly ILogger _logger = logger;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IUserService _userService = userService;
        private readonly IImageService _imageService = imageService;

        public async Task Handle(CustomUpdateAccountAvatar request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authorized");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.NotFound);
            }
            var user = await _dbContext.Users.Include(e => e.Avatar)
                                                .ThenInclude(e => e.PresetAvatar)
                                             .Include(e => e.Avatar)
                                                .ThenInclude(e => e.CustomAvatar)
                                             .Where(e => e.Id == userId)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (user.Avatar == null)
            {
                var avatar = new UserAvatar();
                var imageGroup = await _imageService.SaveImage(request.Avatar, ImageSavingTypes.UserAvatar, cancellationToken);
                foreach(var image in imageGroup.Images)
                {
                    _dbContext.Images.Add(image);
                }
                user.Avatar = avatar;
                avatar.CustomAvatar = imageGroup;
                _dbContext.ImageGroups.Add(imageGroup);
                _dbContext.UserAvatars.Add(avatar);
            }
            else
            {
                if(user.Avatar.CustomAvatar != null)
                {
                    var imageGroup = await _dbContext.ImageGroups.Where(e => e.Id == user.Avatar.CustomAvatarId)
                                                                 .Include(e => e.Images)
                                                                 .FirstOrDefaultAsync();
                    if (imageGroup != null)
                    {
                        await _imageService.RemoveImageGroup(imageGroup, cancellationToken);
                        foreach (var image in imageGroup.Images)
                        {
                            _dbContext.Images.Remove(image);
                        }
                        _dbContext.ImageGroups.Remove(imageGroup);
                    }

                    var newimageGroup = await _imageService.SaveImage(request.Avatar, ImageSavingTypes.UserAvatar, cancellationToken);
                    foreach (var image in newimageGroup.Images)
                    {
                        _dbContext.Images.Add(image);
                    }
                    user.Avatar.CustomAvatar = newimageGroup;
                    _dbContext.ImageGroups.Add(newimageGroup);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
