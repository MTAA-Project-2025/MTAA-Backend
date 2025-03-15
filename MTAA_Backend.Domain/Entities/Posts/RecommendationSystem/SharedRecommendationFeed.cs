using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Posts.RecommendationSystem
{
    public class SharedRecommendationFeed : BaseRecommendationFeed
    {
        public ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
