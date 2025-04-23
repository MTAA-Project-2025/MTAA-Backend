using Google.Protobuf.WellKnownTypes;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.DTOs.RecommendationSystem.Requests;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Posts.Embeddings;
using MTAA_Backend.Domain.Resources.Posts.RecommendationSystem;
using MTAA_Backend.Infrastructure;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using System.Diagnostics.Metrics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels.RealtimeEventTypes;
using static Qdrant.Client.Grpc.Conditions;

namespace MTAA_Backend.Application.Services.RecommendationSystem.RecommendationFeedServices
{
    public class PostsFromPreferencesRecommendationFeedService : IPostsFromPreferencesRecommendationFeedService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IRecommendationItemsService _recommendationItemsService;
        private readonly IVectorDatabaseRepository _vectorDatabaseRepository;

        public const int MaxFeedCount = 200;
        public const double TextWidth = 0.3;
        public const double ImagesWeight = 0.7;
        public const double Weight = 0.6;
        public PostsFromPreferencesRecommendationFeedService(MTAA_BackendDbContext dbContext,
            IRecommendationItemsService recommendationItemsService,
            IVectorDatabaseRepository vectorDatabaseRepository)
        {
            _dbContext = dbContext;
            _recommendationItemsService = recommendationItemsService;
            _vectorDatabaseRepository = vectorDatabaseRepository;
        }
        public async Task AddFeed(string userId, CancellationToken cancellationToken = default)
        {
            var feed = await _dbContext.LocalRecommendationFeeds.Where(e => e.UserId == userId && e.Type == RecommendationFeedTypes.PostsFromPreferencesFeed)
                                                               .FirstOrDefaultAsync(cancellationToken);
            if (feed != null) return;

            feed = new LocalRecommendationFeed()
            {
                UserId = userId,
                IsActive = true,
                Type = RecommendationFeedTypes.PostsFromPreferencesFeed,
                Weight = Weight
            };
            _dbContext.LocalRecommendationFeeds.Add(feed);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task RecomendPostsBackgroundJob(CancellationToken cancellationToken = default)
        {
            var feeds = await _dbContext.LocalRecommendationFeeds.Where(e => e.Type == RecommendationFeedTypes.PostsFromPreferencesFeed && e.IsActive)
                                                    .Include(e => e.RecommendationItems)
                                                    .ToListAsync(cancellationToken);

            foreach (var feed in feeds)
            {
                await RecomendPostsBackgroundJobByFeed(feed, cancellationToken);
            }
        }
        public async Task RecomendPostsBackgroundJobByFeed(LocalRecommendationFeed feed, CancellationToken cancellationToken = default)
        {
            int loadedCount = MaxFeedCount - feed.RecommendationItems.Count;
            int textCount = (int)(loadedCount * TextWidth);
            int imagesCount = (int)(loadedCount * ImagesWeight);

            var filter = new Filter { MustNot = { MatchKeyword("watched[]", feed.UserId) } };

            var userTextVectorRes = await _vectorDatabaseRepository.GetUserPostVector(VectorCollections.UsersPostTextVectors, feed.UserId);
            var userImageVectorRes = await _vectorDatabaseRepository.GetUserPostVector(VectorCollections.UsersPostImageVectors, feed.UserId);

            var textsRes = await _vectorDatabaseRepository.GetPostVectors(VectorCollections.PostTextEmbeddings, userTextVectorRes.Vectors.Vector.Data.ToArray(), (ulong)textCount, feed.UserId, cancellationToken: cancellationToken);
            var imagesRes = await _vectorDatabaseRepository.GetPostVectors(VectorCollections.PostImageEmbeddings, userImageVectorRes.Vectors.Vector.Data.ToArray(), (ulong)imagesCount, feed.UserId, cancellationToken: cancellationToken);

            var request = new List<SimpleAddRecommendationItemRequest>(textCount + imagesCount);
            foreach (var textRes in textsRes)
            {
                await _vectorDatabaseRepository.UpdatePostWatched(Guid.Parse(textRes.Id.Uuid), feed.UserId, cancellationToken);

                request.Add(new SimpleAddRecommendationItemRequest
                {
                    PostId = Guid.Parse(textRes.Id.Uuid),
                    LocalScore = textRes.Score
                });
            }
            foreach (var imageRes in imagesRes)
            {
                await _vectorDatabaseRepository.UpdatePostWatched(Guid.Parse(imageRes.Id.Uuid), feed.UserId, cancellationToken);

                request.Add(new SimpleAddRecommendationItemRequest
                {
                    PostId = Guid.Parse(imageRes.Id.Uuid),
                    LocalScore = imageRes.Score
                });
            }
            await _recommendationItemsService.AddPostsToLocalFeed(RecommendationFeedTypes.PostsFromPreferencesFeed, feed.UserId, request, cancellationToken);

        }

        public async Task<ICollection<Post>> GetRealTimeRecommendations(string userId, int count, CancellationToken cancellationToken = default)
        {
            int textCount = (int)(count * TextWidth);
            int imagesCount = (int)(count * ImagesWeight);

            var userTextVectorRes = await _vectorDatabaseRepository.GetUserPostVector(VectorCollections.UsersPostTextVectors, userId);
            var userImageVectorRes = await _vectorDatabaseRepository.GetUserPostVector(VectorCollections.UsersPostImageVectors, userId);

            var textsRes = await _vectorDatabaseRepository.GetPostVectors(VectorCollections.PostTextEmbeddings, userTextVectorRes.Vectors.Vector.Data.ToArray(), (ulong)textCount, userId, cancellationToken: cancellationToken);
            var imagesRes = await _vectorDatabaseRepository.GetPostVectors(VectorCollections.PostImageEmbeddings, userImageVectorRes.Vectors.Vector.Data.ToArray(), (ulong)imagesCount, userId, cancellationToken: cancellationToken);
            List<Guid> postIds = new List<Guid>(count);

            foreach (var textRes in textsRes)
            {
                await _vectorDatabaseRepository.UpdatePostWatched(Guid.Parse(textRes.Id.Uuid), userId, cancellationToken);
                postIds.Add(Guid.Parse(textRes.Id.Uuid));
            }
            foreach (var imageRes in imagesRes)
            {
                await _vectorDatabaseRepository.UpdatePostWatched(Guid.Parse(imageRes.Id.Uuid), userId, cancellationToken);
                postIds.Add(Guid.Parse(imageRes.Id.Uuid));
            }

            return await _dbContext.Posts.Where(e => postIds.Contains(e.Id))
                                        .Include(e => e.Location)
                                        .Include(e => e.Owner)
                                            .ThenInclude(e => e.Avatar)
                                                .ThenInclude(e => e.CustomAvatar)
                                                    .ThenInclude(e => e.Images)
                                        .Include(e => e.Owner)
                                            .ThenInclude(e => e.Avatar)
                                                .ThenInclude(e => e.PresetAvatar)
                                                    .ThenInclude(e => e.Images)
                                        .Include(e => e.Images)
                                            .ThenInclude(e => e.Images)
                                        .ToListAsync(cancellationToken);
        }
    }
}
