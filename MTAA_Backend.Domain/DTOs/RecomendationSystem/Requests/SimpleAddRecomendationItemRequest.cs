using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.RecomendationSystem.Requests
{
    public class SimpleAddRecomendationItemRequest
    {
        public Guid PostId { get; set; }
        public double LocalScore { get; set; }
    }
}
