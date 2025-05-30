﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Application.CQRS.Users.Account.Events;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Application.Services;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Entities.Images;
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

namespace MTAA_Backend.Application.CQRS.Users.Account.CommandHandlers
{
    public class CustomUpdateAccountAvatarHandler(ILogger<CustomUpdateAccountAvatarHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IImageService _imageService,
        IMapper _mapper,
        IMediator _mediator) : IRequestHandler<CustomUpdateAccountAvatar, MyImageGroupResponse>
    {
        public async Task<MyImageGroupResponse> Handle(CustomUpdateAccountAvatar request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authorized");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.NotFound);
            }

            double aspectRatio = _imageService.GetImageAspectRatio(request.Avatar);
            if (Math.Abs(aspectRatio - 1.0) >= 0.0001)
            {
                _logger.LogError("Aspect ratio is not 1:1");
                throw new HttpException(_localizer[ErrorMessagesPatterns.ImageFormatNotAllowed], HttpStatusCode.BadRequest);
            }

            var user = await _dbContext.Users.Include(e => e.Avatar)
                                                .ThenInclude(e => e.PresetAvatar)
                                             .Include(e => e.Avatar)
                                                .ThenInclude(e => e.CustomAvatar)
                                             .Where(e => e.Id == userId)
                                             .FirstOrDefaultAsync(cancellationToken);

            var response = new MyImageGroupResponse();
            if (user.Avatar == null)
            {
                var avatar = new UserAvatar();
                var imageGroup = await _imageService.SaveImage(request.Avatar, 0, ImageSavingTypes.UserAvatar, cancellationToken);
                foreach (var image in imageGroup.Images)
                {
                    _dbContext.Images.Add(image);
                }
                user.Avatar = avatar;
                avatar.CustomAvatar = imageGroup;
                _dbContext.ImageGroups.Add(imageGroup);
                _dbContext.UserAvatars.Add(avatar);

                response = _mapper.Map<MyImageGroupResponse>(imageGroup);
            }
            else
            {
                if (user.Avatar.CustomAvatar != null)
                {
                    var imageGroup = await _dbContext.ImageGroups.Where(e => e.Id == user.Avatar.CustomAvatarId)
                                                                 .Include(e => e.Images)
                                                                 .FirstOrDefaultAsync(cancellationToken);
                    if (imageGroup != null)
                    {
                        await _imageService.RemoveImageGroup(imageGroup, cancellationToken);
                        foreach (var image in imageGroup.Images)
                        {
                            _dbContext.Images.Remove(image);
                        }
                        _dbContext.ImageGroups.Remove(imageGroup);
                    }
                }
                else
                {
                    user.Avatar.PresetAvatar = null;
                }
                var newimageGroup = await _imageService.SaveImage(request.Avatar, 0, ImageSavingTypes.UserAvatar, cancellationToken);
                foreach (var image in newimageGroup.Images)
                {
                    _dbContext.Images.Add(image);
                }
                user.Avatar.CustomAvatar = newimageGroup;
                _dbContext.ImageGroups.Add(newimageGroup);

                response = _mapper.Map<MyImageGroupResponse>(newimageGroup);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new AccountUpdateEvent()
            {
                UserId = user.Id
            });

            return response;
        }
    }
}
