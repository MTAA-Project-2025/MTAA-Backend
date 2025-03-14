using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.Services;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Posts.CommandHandlers
{
    public class DeletePostHandler(ILogger<DeletePostHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IImageService _imageService) : IRequestHandler<DeletePost>
    {
        public async Task Handle(DeletePost request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var post = await _dbContext.Posts.Where(e => e.Id == request.Id)
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
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
