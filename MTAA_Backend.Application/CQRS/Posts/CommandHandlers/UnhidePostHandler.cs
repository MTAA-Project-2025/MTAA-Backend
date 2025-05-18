using Hangfire;
using MediatR;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.CQRS.Versions.Command;
using MTAA_Backend.Domain.Entities.Notifications;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Versioning;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Posts.CommandHandlers
{
    public class UnhidePostHandler(MTAA_BackendDbContext _dbContext,
        IStringLocalizer<ErrorMessages> _localizer,
        ILogger<HidePostHandler> _logger,
        IMediator _mediator) : IRequestHandler<UnhidePost>
    {
        public async Task Handle(UnhidePost request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts.FindAsync(request.Id);
            if (post == null)
            {
                _logger.LogError($"post not found {request.Id}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.PostNotFound], HttpStatusCode.NotFound);
            }
            post.IsHidden = false;
            if (post.ScheduleJobId != null)
            {
                BackgroundJob.Delete(post.ScheduleJobId);
            }
            post.Version++;
            post.ScheduleJobId = null;
            post.SchedulePublishDate = null;
            await _dbContext.SaveChangesAsync();

            await _mediator.Send(new IncreaseVersion()
            {
                UserId = post.OwnerId,
                VersionItemType = VersionItemType.AccountPosts
            });
        }
    }
}
