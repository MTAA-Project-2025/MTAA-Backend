using MediatR;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;

namespace MTAA_Backend.Application.CQRS.Posts.EventHandlers
{
    public class UpdatePostEventHandler(IPostsConfigureRecommendationsService _postsConfigureRecommendationsService) : INotificationHandler<UpdatePostEvent>
    {
        public async Task Handle(UpdatePostEvent notification, CancellationToken cancellationToken)
        {
            await _postsConfigureRecommendationsService.InitializeRecommendations(notification.PostId, cancellationToken);
        }
    }
}
