using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.CQRS.Posts.Events;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MTAA_Backend.Application.CQRS.Posts.CommandHandlers
{
    public class AddPostHandler(ILogger<AddPostHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IImageService _imageService,
        IMediator _mediator) : IRequestHandler<AddPost, Guid>
    {
        public async Task<Guid> Handle(AddPost request, CancellationToken cancellationToken)
        {
            var post = new Post()
            {
                Description = request.Description
            };
            string userId = _userService.GetCurrentUserId();
            post.OwnerId = userId;

            bool isSameAspectRatio = _imageService.IsImagesHaveSameAspectRatio(request.Images.Select(e => e.Image).ToList());
            var aspectRatio = _imageService.GetImageAspectRatio(request.Images.First().Image);
            if (!isSameAspectRatio || aspectRatio < 0.5 || aspectRatio > 2)
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
            if (request.SchedulePublishDate != null)
            {
                request.SchedulePublishDate = ((DateTime)request.SchedulePublishDate).ToUniversalTime();
            }
            if (request.SchedulePublishDate != null && request.SchedulePublishDate>DateTime.UtcNow.AddMinutes(1))
            {
                post.IsHidden = true;
                post.SchedulePublishDate = request.SchedulePublishDate;
            }
            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync(cancellationToken);

            if (post.IsHidden)
            {
                TimeSpan delay = (DateTime)request.SchedulePublishDate - DateTime.UtcNow;

                post.ScheduleJobId = BackgroundJob.Schedule(() => UnhidePost(post.Id), delay);
                await _dbContext.SaveChangesAsync();
            }

            await _mediator.Publish(new AddPostEvent()
            {
                PostId = post.Id,
                UserId = userId
            }, cancellationToken);

            return post.Id;
        }

        public async Task UnhidePost(Guid id)
        {
            var command = new UnhidePost()
            {
                Id = id
            };
            await _mediator.Send(command);
        }
    }
}
