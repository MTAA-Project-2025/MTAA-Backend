using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.RecommendationSystem
{
    public interface IPostsConfigureRecommendationsService
    {
        public Task InitializeRecommendations(Guid postId, CancellationToken cancellationToken = default);
    }
}
