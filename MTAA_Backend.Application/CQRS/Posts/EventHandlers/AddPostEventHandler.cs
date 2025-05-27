using Hangfire;
using MediatR;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Application.CQRS.Versions.Command;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Versioning;

namespace MTAA_Backend.Application.CQRS.Posts.EventHandlers
{
    /// <summary>
    /// Handles the <see cref="AddPostEvent"/> to trigger post recommendations and update user versioning.
    /// </summary>
    public class AddPostEventHandler(IPostsFromFollowersRecommendationFeedService _recommendationService,
        IPostsConfigureRecommendationsService _postsConfigureRecommendationsService,
        IMediator _mediator) : INotificationHandler<AddPostEvent>
    {
        /// <summary>
        /// Handles the <see cref="AddPostEvent"/> asynchronous.
        /// </summary>
        /// <param name="notification">The <see cref="AddPostEvent"/> notification.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Handle(AddPostEvent notification, CancellationToken cancellationToken)
        {
            await _recommendationService.RecomendPost(notification.PostId, notification.UserId, cancellationToken);
            await _postsConfigureRecommendationsService.InitializeRecommendations(notification.PostId, cancellationToken);
            await _mediator.Send(new IncreaseVersion()
            {
                UserId = notification.UserId,
                VersionItemType = VersionItemType.AccountPosts
            });
        }
    }
}
