using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Users.Identity.Events;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Posts.Embeddings;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Users.Identity.EventHandlers
{
    public class CreateAccountEventHandler(IAccountService _accountService,
        MTAA_BackendDbContext _dbContext,
        ILogger<CreateAccountEventHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        IPostsFromFollowersRecommendationFeedService _fromFollowersRecommendationService,
        IPostsFromGlobalPopularityRecommendationFeedService _fromGlobalPopularityRecommendationService,
        IPostsFromPreferencesRecommendationFeedService _fromPreferencesRecommendationService,
        IVectorDatabaseRepository _vectorDatabaseRepository,
        IVersionItemService _versionItemService) : INotificationHandler<CreateAccountEvent>
    {
        public async Task Handle(CreateAccountEvent notification, CancellationToken cancellationToken)
        {
            var imageGroup = await _dbContext.UserPresetAvatarImages.Where(e => e.Id == PresetAvatarImages.Image1Id)
                                                        .Include(e => e.Images)
                                                        .FirstOrDefaultAsync(cancellationToken);

            var user = await _dbContext.Users.Where(e => e.Id == notification.UserId)
                                             .Include(e => e.Avatar)
                                                .ThenInclude(e => e.CustomAvatar)
                                             .Include(e => e.Avatar)
                                                .ThenInclude(e => e.PresetAvatar)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                _logger.LogError($"User not found {notification.UserId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound], HttpStatusCode.BadRequest);
            }

            await _accountService.ChangePresetAvatar(imageGroup, user, cancellationToken);

            await _fromFollowersRecommendationService.AddFeed(user.Id, cancellationToken);
            await _fromGlobalPopularityRecommendationService.AddFeed(user.Id, cancellationToken);
            await _fromPreferencesRecommendationService.AddFeed(user.Id, cancellationToken);

            await _vectorDatabaseRepository.AddUserPostVector(VectorCollections.UsersPostImageVectors, user.Id);
            await _vectorDatabaseRepository.AddUserPostVector(VectorCollections.UsersPostTextVectors, user.Id);

            await _versionItemService.InitializationForUserAsync(notification.UserId, cancellationToken);
        }
    }
}
