using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.DTOs.RecommendationSystem.Requests;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Posts.RecommendationSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.RecommendationSystem
{
    public interface IRecommendationItemsService
    {
        public Task AddPostsToLocalFeed(RecommendationFeedTypes feedType, string userId, ICollection<SimpleAddRecommendationItemRequest> requests, CancellationToken cancellationToken = default);
        public Task RemovePostsFromLocalFeed(RecommendationFeedTypes feedType, string userId, ICollection<Guid> postIds, CancellationToken cancellationToken = default);

        public Task AddPostsToSharedFeed(RecommendationFeedTypes feedType, ICollection<SimpleAddRecommendationItemRequest> requests, CancellationToken cancellationToken = default);
        public Task ClearSharedFeed(RecommendationFeedTypes feedType, CancellationToken cancellationToken = default);
        public Task RemovePostsFromSharedFeed(RecommendationFeedTypes feedType, ICollection<Guid> postIds, CancellationToken cancellationToken = default);
    }
}
