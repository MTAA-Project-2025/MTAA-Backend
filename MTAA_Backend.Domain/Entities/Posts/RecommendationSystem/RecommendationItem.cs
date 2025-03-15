using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Posts.RecommendationSystem
{
    public class RecommendationItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public double LocalScore { get; set; }

        public Post Post { get; set; }
        public Guid PostId { get; set; }

        public BaseRecommendationFeed Feed { get; set; }
        public Guid FeedId { get; set; }
    }
}
