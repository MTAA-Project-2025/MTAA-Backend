using Google.Protobuf.WellKnownTypes;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.DTOs.RecommendationSystem.Requests;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Posts.Embeddings;
using MTAA_Backend.Domain.Resources.Posts.RecommendationSystem;
using MTAA_Backend.Infrastructure;
using Nest;
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
    /// <summary>
    /// Provides services for managing recommendation feeds based on user preferences using vector-based similarity.
    /// </summary>
    public class PostsFromPreferencesRecommendationFeedService : IPostsFromPreferencesRecommendationFeedService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IRecommendationItemsService _recommendationItemsService;
        private readonly IVectorDatabaseRepository _vectorDatabaseRepository;

        /// <summary>
        /// The maximum number of posts to include in the feed.
        /// </summary>
        public const int MaxFeedCount = 200;

        /// <summary>
        /// The weight for text-based recommendations.
        /// </summary>
        public const double TextWidth = 0.3;

        /// <summary>
        /// The weight for image-based recommendations.
        /// </summary>
        public const double ImagesWeight = 0.7;

        /// <summary>
        /// The weight assigned to the preferences-based feed.
        /// </summary>
        public const double Weight = 0.6;

        /// <summary>
        /// Initializes a new instance of the PostsFromPreferencesRecommendationFeedService class.
        /// </summary>
        /// <param name="dbContext">The database context for data operations.</param>
        /// <param name="recommendationItemsService">The service for managing recommendation items.</param>
        /// <param name="vectorDatabaseRepository">The repository for vector database operations.</param>
        public PostsFromPreferencesRecommendationFeedService(MTAA_BackendDbContext dbContext,
            IRecommendationItemsService recommendationItemsService,
            IVectorDatabaseRepository vectorDatabaseRepository)
        {
            _dbContext = dbContext;
            _recommendationItemsService = recommendationItemsService;
            _vectorDatabaseRepository = vectorDatabaseRepository;
        }

        /// <summary>
        /// Adds a preferences-based recommendation feed for a user if it does not exist.
        /// </summary>
        /// <param name="userId">The ID of the user to add the feed for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Background job to recommend posts for all active preferences-based feeds.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Background job to recommend posts for a specific preferences-based feed.
        /// </summary>
        /// <param name="feed">The recommendation feed to process.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RecomendPostsBackgroundJobByFeed(LocalRecommendationFeed feed, CancellationToken cancellationToken = default)
        {
            int loadedCount = MaxFeedCount - feed.RecommendationItems.Count;
            if (loadedCount <= 0) return;
            int textCount = (int)(loadedCount * TextWidth);
            int imagesCount = (int)(loadedCount * ImagesWeight);

            var filter = new Qdrant.Client.Grpc.Filter { MustNot = { MatchKeyword("watched[]", feed.UserId) } };

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

        /// <summary>
        /// Retrieves real-time post recommendations based on user preferences.
        /// </summary>
        /// <param name="userId">The ID of the user to get recommendations for.</param>
        /// <param name="count">The number of recommendations to retrieve.</param>
        /// <param name="isStrict">Whether to apply strict filtering.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning a collection of post IDs.</returns>
        public async Task<ICollection<Guid>> GetRealTimeRecommendations(string userId, int count, bool isStrict = true, CancellationToken cancellationToken = default)
        {
            int textCount = (int)(Math.Round(count * TextWidth));
            int imagesCount = (int)(Math.Round(count * ImagesWeight));

            var userTextVectorRes = await _vectorDatabaseRepository.GetUserPostVector(VectorCollections.UsersPostTextVectors, userId);
            var userImageVectorRes = await _vectorDatabaseRepository.GetUserPostVector(VectorCollections.UsersPostImageVectors, userId);

            var textsRes = await _vectorDatabaseRepository.GetPostVectors(VectorCollections.PostTextEmbeddings, userTextVectorRes.Vectors.Vector.Data.ToArray(), (ulong)textCount, userId, isStrict: isStrict, cancellationToken: cancellationToken);
            var imagesRes = await _vectorDatabaseRepository.GetPostVectors(VectorCollections.PostImageEmbeddings, userImageVectorRes.Vectors.Vector.Data.ToArray(), (ulong)imagesCount, userId, isStrict: isStrict, cancellationToken: cancellationToken);
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
            return postIds;
        }

        /// <summary>
        /// Updates user preferences based on interaction with a post.
        /// </summary>
        /// <param name="userId">The ID of the user whose preferences are updated.</param>
        /// <param name="postId">The ID of the post interacted with.</param>
        /// <param name="k">The scaling factor for the update.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ChangeReccommendations(string userId, Guid postId, float k, CancellationToken cancellationToken = default)
        {
            var userTextVectorRes = (await _vectorDatabaseRepository.GetUserPostVector(VectorCollections.UsersPostTextVectors, userId)).Vectors.Vector.Data.ToArray();
            var userImageVectorRes = (await _vectorDatabaseRepository.GetUserPostVector(VectorCollections.UsersPostImageVectors, userId)).Vectors.Vector.Data.ToArray();

            var postTextVector = (await _vectorDatabaseRepository.GetPostVector(VectorCollections.PostTextEmbeddings, postId)).Vectors.Vector.Data.ToArray();
            var postImageVector = (await _vectorDatabaseRepository.GetPostVector(VectorCollections.PostTextEmbeddings, postId)).Vectors.Vector.Data.ToArray();

            float[] textArr = new float[userTextVectorRes.Length];
            float[] imageArr = new float[userImageVectorRes.Length];
            for (int i =0; i < userImageVectorRes.Length; i++)
            {
                textArr[i] = userTextVectorRes[i] + postTextVector[i] * k;
            }

            for (int i = 0; i < userImageVectorRes.Length; i++)
            {
                imageArr[i] = userImageVectorRes[i] + postImageVector[i] * k;
            }

            await _vectorDatabaseRepository.UpdateUserPostVector(VectorCollections.UsersPostTextVectors, userId, textArr);
            await _vectorDatabaseRepository.UpdateUserPostVector(VectorCollections.UsersPostImageVectors, userId, imageArr);
        }
    }
}
