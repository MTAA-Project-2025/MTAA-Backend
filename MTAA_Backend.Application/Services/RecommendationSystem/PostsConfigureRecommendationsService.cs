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
    public class PostsConfigureRecommendationsService : IPostsConfigureRecommendationsService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly ILogger<PostsConfigureRecommendationsService> _logger;
        private readonly IStringLocalizer<ErrorMessages> _localizer;
        private readonly IEmbeddingsService _embeddingsService;
        private readonly QdrantClient _qdrantClient;
        private readonly IBackgroundJobClient _backgroundJobClient;
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
