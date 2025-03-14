using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Posts.RecommendationSystem
{
    public class LocalRecommendationFeed : BaseRecommendationFeed
    {
        public User User { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
