using Hangfire;
using MediatR;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;

namespace MTAA_Backend.Application.CQRS.Posts.EventHandlers
{
    public class AddPostEventHandler(IPostsFromFollowersRecommendationFeedService _RecommendationService,
        IPostsConfigureRecommendationsService _postsConfigureRecommendationsService) : INotificationHandler<AddPostEvent>
    {
        public async Task Handle(AddPostEvent notification, CancellationToken cancellationToken)
        {
            await _RecommendationService.RecomendPost(notification.PostId, notification.UserId, cancellationToken);
            await _postsConfigureRecommendationsService.InitializeRecommendations(notification.PostId, cancellationToken);
        }
    }
}
