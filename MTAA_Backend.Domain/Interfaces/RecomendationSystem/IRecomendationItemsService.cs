using Azure.Storage.Blobs.Models;
using MTAA_Backend.Domain.DTOs.RecomendationSystem.Requests;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Resources.Posts.RecomendationSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.RecomendationSystem
{
    public interface IRecomendationItemsService
    {
        public Task AddPostsToFeed(RecomendationFeedTypes feedType, string userId, ICollection<SimpleAddRecomendationItemRequest> requests, CancellationToken cancellationToken = default);
        public Task RemovePostsFromFeed(RecomendationFeedTypes feedType, string userId, ICollection<Guid> postIds, CancellationToken cancellationToken = default);
    }
}
