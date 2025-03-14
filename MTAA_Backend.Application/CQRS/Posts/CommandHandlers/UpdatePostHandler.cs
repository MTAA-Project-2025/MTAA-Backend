using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Domain.DTOs.Images.Requests;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Posts.CommandHandlers
{
    public class UpdatePostHandler(ILogger<UpdatePostHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IImageService _imageService,
        IMediator _mediator) : IRequestHandler<UpdatePost>
    {
        public async Task Handle(UpdatePost request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts.Where(e => e.Id == request.Id)
                                             .Include(e => e.Images)
                                                .ThenInclude(e => e.Images)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (post == null)
            {
                _logger.LogError($"Post {request.Id} not found");
                throw new HttpException(_localizer[ErrorMessagesPatterns.PostNotFound], HttpStatusCode.NotFound);
            }

            var images = request.Images.Where(e => e.NewImage != null).Select(e => e.NewImage).ToList();

            bool isSameAspectRatio = true;
            double standardAspectRatio = post.Images.First().Images.Where(e => e.Type == ImageSizeType.Middle).First().AspectRatio;

            foreach(var image in images)
            {
                double aspectRatio = _imageService.GetImageAspectRatio(image);
                if (Math.Abs(standardAspectRatio - aspectRatio) >= 0.001)
                {
                    isSameAspectRatio = false;
                    break;
                }
            }

            if (!isSameAspectRatio)
            {
                _logger.LogError("Image aspect ratio is not allowed");
                throw new HttpException(_localizer[ErrorMessagesPatterns.ImageFormatNotAllowed], HttpStatusCode.BadRequest);
            }
            var newImages = request.Images.Where(e => e.NewImage != null);
            var addRequests = new List<AddImageRequest>(newImages.Count());
            foreach (var newImage in newImages)
            {
                var oldImage = post.Images.FirstOrDefault(e => e.Position == newImage.Position);
                if (oldImage != null)
                {
                    await _imageService.RemoveImageGroup(oldImage, cancellationToken);
                    _dbContext.ImageGroups.Remove(oldImage);
                }
                addRequests.Add(new AddImageRequest()
                {
                    Image = newImage.NewImage,
                    Position = newImage.Position
                });
            }

            var imageGroups = await _imageService.SaveImages(addRequests, ImageSavingTypes.PostImage);

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

            await _mediator.Publish(new UpdatePostEvent()
            {
                PostId = post.Id,
            }, cancellationToken);
        }
    }
}
