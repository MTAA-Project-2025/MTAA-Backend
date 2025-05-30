﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Application.Services;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Interfaces.Locations;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Posts.CommandHandlers
{
    public class DeletePostHandler(ILogger<DeletePostHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IImageService _imageService,
        ILocationService _locationService,
        IMediator _mediator) : IRequestHandler<DeletePost>
    {
        public async Task Handle(DeletePost request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var post = await _dbContext.Posts.Where(e => e.Id == request.Id)
                                             .Include(e => e.RecommendationItems)
                                             .Include(e => e.Location)
                                                .ThenInclude(e => e.Points)
                                             .Include(e => e.Images)
                                                 .ThenInclude(e => e.Images)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (post == null)
            {
                _logger.LogError($"post not found {request.Id}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.PostNotFound], HttpStatusCode.NotFound);
            }

            foreach(var image in post.Images)
            {
                await _imageService.RemoveImageGroup(image);
            }
            _dbContext.Posts.Remove(post);

            if (post.Location != null)
            {
                await _locationService.DeletePoints(post.Location);
                _dbContext.Locations.Remove(post.Location);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new DeletePostEvent()
            {
                PostId = post.Id,
                UserId = userId
            });
        }
    }
}
