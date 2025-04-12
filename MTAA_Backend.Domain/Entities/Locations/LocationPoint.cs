using MTAA_Backend.Domain.Resources.Other;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Locations
{
    public class LocationPoint
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsSubPoint { get; set; }
        public bool IsVisible { get; set; } = true;

        public Location? Location { get; set; }
        public Guid? LocationId { get; set; }

        public int ZoomLevel { get; set; }
        public LocationPointType Type { get; set; }

        public LocationPoint? Parent { get; set; }

        public Guid? ParentId { get; set; }
        public Point Coordinates { get; set; }

        public ICollection<LocationPoint> LocationPoints { get; set; } = new HashSet<LocationPoint>();
    }
}
