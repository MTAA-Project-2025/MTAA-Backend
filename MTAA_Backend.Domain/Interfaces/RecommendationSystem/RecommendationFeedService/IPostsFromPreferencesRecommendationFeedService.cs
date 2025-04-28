using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService
{
    public interface IPostsFromPreferencesRecommendationFeedService
    {
        public Task AddFeed(string userId, CancellationToken cancellationToken = default);
        public Task RecomendPostsBackgroundJob(CancellationToken cancellationToken = default);
        public Task RecomendPostsBackgroundJobByFeed(LocalRecommendationFeed feed, CancellationToken cancellationToken = default);
        public Task<ICollection<Guid>> GetRealTimeRecommendations(string userId, int count, bool isStrict = true, CancellationToken cancellationToken = default);
    }
}
