using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Groups;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.CommandHandlers
{
    public class AddChannelHandler(ILogger<AddChannelHandler> logger,
        IStringLocalizer<ErrorMessages> localizer,
        MTAA_BackendDbContext dbContext,
        IImageService imageService,
        IUserService userService) : IRequestHandler<AddChannel, Guid>
    {
        private readonly ILogger _logger = logger;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IImageService _imageService = imageService;
        private readonly IUserService _userService = userService;

        public async Task<Guid> Handle(AddChannel request, CancellationToken cancellationToken)
        {
            var visibilities = GroupVisibilityTypes.GetAllPublic();
            if (!visibilities.Contains(request.Visibility))
            {
                _logger.LogError($"Visibility not found: {request.Visibility}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.GroupVisibilityTypeDontExist], HttpStatusCode.BadRequest);
            }

            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authorized");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.NotFound);
            }

            var newChannel = new Channel()
            {
                DisplayName = request.DisplayName,
                IdentificationName = request.IdentificationName,
                Description = request.Description,
                Visibility = request.Visibility,
                OwnerId = userId
            };

            var channel = await _dbContext.Channels.Where(e => e.IdentificationName == request.IdentificationName)
                                                   .FirstOrDefaultAsync(cancellationToken);

            if (channel != null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.ChannelAlreadyExist], HttpStatusCode.BadRequest);
            }

            if (request.Image != null)
            {
                var imageGroup = await _imageService.SaveImage(request.Image,0, ImageSavingTypes.ChannelImage, cancellationToken);
                foreach (var image in imageGroup.Images)
                {
                    _dbContext.Images.Add(image);
                }
                _dbContext.ImageGroups.Add(imageGroup);
                newChannel.Image = imageGroup;
            }

            _dbContext.Channels.Add(newChannel);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return newChannel.Id;
        }
    }
}
