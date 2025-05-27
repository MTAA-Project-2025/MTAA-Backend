using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Posts.Embeddings;
using MTAA_Backend.Infrastructure;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using System.Net;
using System.Threading;
using static Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels.RealtimeEventTypes;

namespace MTAA_Backend.Application.Services.RecommendationSystem
{
    /// <summary>
    /// Provides services for initializing post recommendations by generating and storing embeddings.
    /// </summary>
    public class PostsConfigureRecommendationsService : IPostsConfigureRecommendationsService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly ILogger<PostsConfigureRecommendationsService> _logger;
        private readonly IStringLocalizer<ErrorMessages> _localizer;
        private readonly IEmbeddingsService _embeddingsService;
        private readonly QdrantClient _qdrantClient;
        private readonly IBackgroundJobClient _backgroundJobClient;

        /// <summary>
        /// Initializes a new instance of the PostsConfigureRecommendationsService class.
        /// </summary>
        /// <param name="dbContext">The database context for data operations.</param>
        /// <param name="logger">The logger for recording errors.</param>
        /// <param name="localizer">The localizer for error messages.</param>
        /// <param name="embeddingsService">The service for generating embeddings.</param>
        /// <param name="qdrantClient">The client for interacting with the Qdrant vector database.</param>
        /// <param name="backgroundJobClient">The client for scheduling background jobs.</param>
        public PostsConfigureRecommendationsService(MTAA_BackendDbContext dbContext,
            ILogger<PostsConfigureRecommendationsService> logger,
            IStringLocalizer<ErrorMessages> localizer,
            IEmbeddingsService embeddingsService,
            QdrantClient qdrantClient,
            IBackgroundJobClient backgroundJobClient)
        {
            _dbContext = dbContext;
            _logger = logger;
            _localizer = localizer;
            _qdrantClient = qdrantClient;
            _backgroundJobClient = backgroundJobClient;
            _embeddingsService = embeddingsService;
        }

        /// <summary>
        /// Initializes recommendations for a post by scheduling a background job.
        /// </summary>
        /// <param name="postId">The ID of the post to initialize recommendations for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="HttpException">Thrown if the post is not found.</exception>
        public async Task InitializeRecommendations(Guid postId, CancellationToken cancellationToken = default)
        {
            var post = await _dbContext.Posts.FindAsync(postId, cancellationToken);

            if (post == null)
            {
                _logger.LogError($"Post {postId} not found");
                throw new HttpException(_localizer[ErrorMessagesPatterns.PostNotFound], HttpStatusCode.NotFound);
            }

            _backgroundJobClient.Enqueue(() => InitializeRecommendationsBackgroundJob(postId));
        }

        /// <summary>
        /// Background job to generate and store text and image embeddings for a post in the vector database.
        /// </summary>
        /// <param name="postId">The ID of the post to process.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeRecommendationsBackgroundJob(Guid postId)
        {
            var post = await _dbContext.Posts.Where(e => e.Id == postId)
                           .Include(e => e.Images)
                           .ThenInclude(e => e.Images)
                           .FirstOrDefaultAsync();

            var textEmbedding = (await _embeddingsService.GetTextEmbeddings(post.Description)).Select(x => (float)x).ToArray();

            ListValue listValue = new ListValue();
            listValue.Values.Add(post.OwnerId);
            await _qdrantClient.UpsertAsync(collectionName: VectorCollections.PostTextEmbeddings, points: new List<PointStruct>
            {
                new PointStruct()
                {
                    Id = post.Id,
                    Vectors = textEmbedding,
                    Payload = {
                        ["watched"] = new Value()
                        {
                            ListValue=listValue
                        }
                    }
                }
            });

            var image = post.Images.First().Images.First(e => e.Type == ImageSizeType.Small);
            var imageEmbedding = await _embeddingsService.GetImageEmbeddings(image);
            await _qdrantClient.UpsertAsync(collectionName: VectorCollections.PostImageEmbeddings, points: new List<PointStruct>
            {
                new PointStruct()
                {
                    Id = post.Id,
                    Vectors = imageEmbedding,
                    Payload = {
                        ["watched"] = new Value()
                        {
                            ListValue=listValue
                        }
                    }
                }
            });
        }
    }
}
