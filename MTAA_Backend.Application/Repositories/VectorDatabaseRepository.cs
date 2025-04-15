using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Posts.Embeddings;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using System.Numerics;
using static Qdrant.Client.Grpc.Conditions;

namespace MTAA_Backend.Application.Repositories
{
    public class VectorDatabaseRepository : IVectorDatabaseRepository
    {
        private readonly QdrantClient _qdrantClient;
        public VectorDatabaseRepository(QdrantClient qdrantClient)
        {
            _qdrantClient = qdrantClient;
        }

        public async Task<IReadOnlyList<ScoredPoint>> GetPostVectors(string collectionName, float[] userVector, ulong limit, string? userId, ulong offset = 0, CancellationToken cancellationToken = default)
        {
            Filter filter = null;
            if (userId != null)
            {
                filter = new Filter { MustNot = { MatchKeyword("watched[]", userId) } };
            }
            return await _qdrantClient.SearchAsync(collectionName: collectionName,
                vector: userVector,
                limit: limit,
                filter: filter,
                offset: offset,
                cancellationToken: cancellationToken);
        }
        public async Task UpdatePostWatched(Guid postId, string userId, CancellationToken cancellationToken = default)
        {
            var textVector = (await _qdrantClient.QueryAsync(
                collectionName: VectorCollections.PostTextEmbeddings,
                query: postId,
                payloadSelector: true,
                cancellationToken: cancellationToken
            )).FirstOrDefault();

            if (textVector != null)
            {
                textVector.Payload["watched"].ListValue.Values.Add(userId);
                if (textVector.Payload["watched"].ListValue.Values.Any(e => e.StringValue == userId)) return;
                await _qdrantClient.OverwritePayloadAsync(
                    collectionName: VectorCollections.PostTextEmbeddings,
                    payload: textVector.Payload,
                    id: postId,
                    cancellationToken: cancellationToken
                );
            }

            var imageVector = (await _qdrantClient.QueryAsync(
                collectionName: VectorCollections.PostImageEmbeddings,
                query: postId,
                payloadSelector: true,
                cancellationToken: cancellationToken
            )).FirstOrDefault();

            if (imageVector != null)
            {
                if (imageVector.Payload["watched"].ListValue.Values.Any(e => e.StringValue == userId)) return;
                imageVector.Payload["watched"].ListValue.Values.Add(userId);
                await _qdrantClient.OverwritePayloadAsync(
                    collectionName: VectorCollections.PostImageEmbeddings,
                    payload: imageVector.Payload,
                    id: postId,
                    cancellationToken: cancellationToken
                );
            }
        }

        public async Task AddUserPostVector(string collectionName, string userId)
        {
            var randomVector = (await _qdrantClient.QueryAsync(
                    collectionName: collectionName,
                    query: Sample.Random,
                    vectorsSelector: true
                )).FirstOrDefault();

            var vector = randomVector.Vectors.Vector.Data.ToArray();

            await _qdrantClient.UpsertAsync(collectionName: collectionName,
                points: new List<PointStruct>
                {
                    new PointStruct()
                    {
                        Id = Guid.Parse(userId),
                        Vectors = vector,
                    }
                }
            );
        }

        public async Task<ScoredPoint> GetUserPostVector(string collectionName, string userId)
        {
            var userVector = (await _qdrantClient.QueryAsync(
                collectionName: collectionName,
                query: Guid.Parse(userId),
                vectorsSelector: true
            )).FirstOrDefault();

            return userVector;
        }

        public async Task RemovePostVectors(Guid postId)
        {
            await _qdrantClient.DeleteAsync(VectorCollections.PostImageEmbeddings, id: postId);
            await _qdrantClient.DeleteAsync(VectorCollections.PostTextEmbeddings, id: postId);
        }
    }
}
