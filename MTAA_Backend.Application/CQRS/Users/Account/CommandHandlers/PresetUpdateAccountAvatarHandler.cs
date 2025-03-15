using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Application.Services;
using MTAA_Backend.Domain.DTOs.Images.Response;
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

namespace MTAA_Backend.Application.CQRS.Users.Account.CommandHandlers
{
    public class PresetUpdateAccountAvatarHandler(ILogger<PresetUpdateAccountAvatarHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IAccountService _accountService,
        IMapper _mapper) : IRequestHandler<PresetUpdateAccountAvatar, MyImageGroupResponse>
    {
        public async Task<MyImageGroupResponse> Handle(PresetUpdateAccountAvatar request, CancellationToken cancellationToken)
        {
            var imageGroup = await _dbContext.UserPresetAvatarImages.Where(e => e.Id == request.ImageGroupId)
                                                                    .Include(e => e.Images)
                                                                    .FirstOrDefaultAsync(cancellationToken);
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

            await _accountService.ChangePresetAvatar(imageGroup, user, cancellationToken);

            return _mapper.Map<MyImageGroupResponse>(imageGroup);
        }
    }
}
