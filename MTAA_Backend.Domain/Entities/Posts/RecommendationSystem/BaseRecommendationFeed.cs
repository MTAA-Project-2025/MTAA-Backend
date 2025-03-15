using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Resources.Posts.RecommendationSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Posts.RecommendationSystem
{
    public class BaseRecommendationFeed
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public RecommendationFeedTypes Type { get; set; }
        public double Weight { get; set; }
        public int RecommendationItemsCount { get; set; } = 0;

        public ICollection<RecommendationItem> RecommendationItems { get; set; } = new HashSet<RecommendationItem>();
    }
}
