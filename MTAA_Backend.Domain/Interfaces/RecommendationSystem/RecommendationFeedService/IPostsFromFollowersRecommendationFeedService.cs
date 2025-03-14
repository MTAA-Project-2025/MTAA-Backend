using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService
{
    public interface IPostsFromFollowersRecommendationFeedService
    {
        public Task AddFeed(string userId, CancellationToken cancellationToken = default);
        public Task RecomendPost(Guid postId, string userId, CancellationToken cancellationToken = default);
    }
}
