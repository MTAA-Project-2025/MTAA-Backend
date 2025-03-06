using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Resources.Posts.RecomendationSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Posts.RecomendationSystem
{
    public class RecomendationFeed
    {
        public Guid Id { get; set; }

        public RecomendationFeedTypes Type { get; set; }
        public double Weight { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public ICollection<RecomendationItem> RecomendationItems { get; set; } = new HashSet<RecomendationItem>();
    }
}
