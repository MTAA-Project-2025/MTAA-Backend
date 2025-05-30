﻿using MTAA_Backend.Domain.Resources.Posts.Embeddings;
using Qdrant.Client.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces
{
    public interface IVectorDatabaseRepository
    {
        public Task<IReadOnlyList<ScoredPoint>> GetPostVectors(string collectionName, float[] userVector, ulong limit, string? userId, ulong offset=0, bool isStrict = true, CancellationToken cancellationToken = default);
        public Task UpdatePostWatched(Guid postId, string userId, CancellationToken cancellationToken = default);

        public Task AddUserPostVector(string collectionName, string userId);
        public Task<ScoredPoint> GetUserPostVector(string collectionName, string userId);

        public Task<ScoredPoint> GetPostVector(string collectionName, Guid postId);
        public Task TaskUpdatePostVector(string collectionName, Guid postId, float[] vector);
        public Task UpdateUserPostVector(string collectionName, string userId, float[] vector);
        public Task RemovePostVectors(Guid postId);
    }
}
