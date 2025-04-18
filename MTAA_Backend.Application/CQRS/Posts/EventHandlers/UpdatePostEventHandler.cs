﻿using MediatR;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Application.CQRS.Versions.Command;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Domain.Resources.Versioning;

namespace MTAA_Backend.Application.CQRS.Posts.EventHandlers
{
    public class UpdatePostEventHandler(IPostsConfigureRecommendationsService _postsConfigureRecommendationsService,
        IMediator _mediator) : INotificationHandler<UpdatePostEvent>
    {
        public async Task Handle(UpdatePostEvent notification, CancellationToken cancellationToken)
        {
            await _postsConfigureRecommendationsService.InitializeRecommendations(notification.PostId, cancellationToken);

            await _mediator.Send(new IncreaseVersion()
            {
                UserId = notification.UserId,
                VersionItemType = VersionItemType.AccountPosts
            });
        }
    }
}
