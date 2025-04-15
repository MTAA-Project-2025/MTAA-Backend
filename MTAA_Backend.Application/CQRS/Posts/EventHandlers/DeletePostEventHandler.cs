using MediatR;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Application.CQRS.Versions.Command;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Domain.Resources.Versioning;
using MTAA_Backend.Domain.Interfaces;

namespace MTAA_Backend.Application.CQRS.Posts.EventHandlers
{
    public class DeletePostEventHandler(IVectorDatabaseRepository _vectorDbRepo,
        IMediator _mediator) : INotificationHandler<DeletePostEvent>
    {
        public async Task Handle(DeletePostEvent notification, CancellationToken cancellationToken)
        {
            await _vectorDbRepo.RemovePostVectors(notification.PostId);
            await _mediator.Send(new IncreaseVersion()
            {
                UserId = notification.UserId,
                VersionItemType = VersionItemType.AccountPosts
            });
        }
    }
}
