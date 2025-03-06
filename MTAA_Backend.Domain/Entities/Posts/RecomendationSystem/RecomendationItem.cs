using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Posts.RecomendationSystem
{
    public class RecomendationItem
    {
        public Guid Id { get; set; }

        public double LocalScore { get; set; }

        public Post Post { get; set; }
        public Guid PostId { get; set; }

        public RecomendationFeed Feed { get; set; }
        public Guid FeedId { get; set; }
    }
}
