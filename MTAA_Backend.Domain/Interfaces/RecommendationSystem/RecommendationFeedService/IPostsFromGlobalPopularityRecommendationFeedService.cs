using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService
{
    public interface IPostsFromGlobalPopularityRecommendationFeedService
    {
        public Task AddFeed(string userId, CancellationToken cancellationToken = default);
        public Task RecomendPostsBackgroundJob(CancellationToken cancellationToken = default);
    }
}
