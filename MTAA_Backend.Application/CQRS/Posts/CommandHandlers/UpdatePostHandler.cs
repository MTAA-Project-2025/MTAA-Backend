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

            if (images.Count > 0)
            {
                bool isSameAspectRatio = _imageService.IsImagesHaveSameAspectRatio(images);
                double newAspectRatio = _imageService.GetImageAspectRatio(images.FirstOrDefault());

                if (isSameAspectRatio)
                {
                    var oldImageIds = request.Images.Where(e => e.NewImage == null && e.OldImageId != null).Select(e => e.OldImageId).ToList();

                    var oldImages = post.Images.Select(e => e.Images.Where(e => e.Type == ImageSizeType.Middle).FirstOrDefault()).Where(e => oldImageIds.Contains(e.Id)).ToList();

                    if (oldImages.Count > 0)
                    {
                        double standardAspectRatio = oldImages.FirstOrDefault().AspectRatio;

                        foreach (var image in images)
                        {
                            double aspectRatio = _imageService.GetImageAspectRatio(image);
                            if (Math.Abs(standardAspectRatio - aspectRatio) >= 0.01)
                            {
                                isSameAspectRatio = false;
                                break;
                            }
                        }
                    }
                }
                

                if (!isSameAspectRatio || newAspectRatio < 0.5 || newAspectRatio > 2)
                {
                    _logger.LogError("Image aspect ratio is not allowed");
                    throw new HttpException(_localizer[ErrorMessagesPatterns.ImageFormatNotAllowed], HttpStatusCode.BadRequest);
                }
            }






            var newImages = request.Images.Where(e => e.NewImage != null).ToList();
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

            post.Description = request.Description;
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new UpdatePostEvent()
            {
                PostId = post.Id,
            }, cancellationToken);
        }
    }
}
