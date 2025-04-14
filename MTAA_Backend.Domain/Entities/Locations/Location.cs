using MTAA_Backend.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Locations
{
    public class Location
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Post? Post { get; set; }
        public Guid? PostId { get; set; }

        public DateTime EventTime { get; set; }

        public ICollection<LocationPoint> Points { get; set; } = new HashSet<LocationPoint>();
    }
}
