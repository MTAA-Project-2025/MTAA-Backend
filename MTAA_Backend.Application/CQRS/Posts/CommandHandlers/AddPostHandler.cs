using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.CQRS.Users.Account.CommandHandlers;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Posts.CommandHandlers
{
    public class AddPostHandler(ILogger<CustomUpdateAccountAvatarHandler> logger,
        IStringLocalizer<ErrorMessages> localizer,
        MTAA_BackendDbContext dbContext,
        IUserService userService,
        IImageService imageService) : IRequestHandler<AddPost, Guid>
    {
        private readonly ILogger _logger = logger;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IUserService _userService = userService;
        private readonly IImageService _imageService = imageService;

        public async Task<Guid> Handle(AddPost request, CancellationToken cancellationToken)
        {
            var post = new Post()
            {
                Description = request.Description
            };
            post.OwnerId = _userService.GetCurrentUserId();

            bool isSameAspectRatio = _imageService.IsImagesHaveSameAspectRatio(request.Images);
            if (!isSameAspectRatio)
            {
                _logger.LogError("Image aspect ratio is not allowed");
                throw new HttpException(_localizer[ErrorMessagesPatterns.ImageFormatNotAllowed], HttpStatusCode.BadRequest);
            }

            var imageGroups = await _imageService.SaveImages(request.Images, ImageSavingTypes.PostImage);

            foreach (var imageGroup in imageGroups)
            {
                post.Images.Add(imageGroup);
                foreach (var image in imageGroup.Images)
                {
                    _dbContext.Images.Add(image);
                }

                _dbContext.ImageGroups.Add(imageGroup);
            }
            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return post.Id;
        }
    }
}
