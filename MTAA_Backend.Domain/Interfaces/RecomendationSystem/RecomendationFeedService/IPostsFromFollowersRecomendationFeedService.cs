using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.RecomendationSystem.RecomendationFeedService
{
    public interface IPostsFromFollowersRecomendationFeedService
    {
        public void RecomendPost(Guid id);
    }
}
