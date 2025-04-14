using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Locations
{
    public struct LocationConstants
    {
        public const double MIN_LATITUDE = -90;
        public const double MAX_LATITUDE = 90;
        
        public const double MIN_LONGITUDE = -180;
        public const double MAX_LONGITUDE = 180;

        public const int MIN_ZOOM_LEVEL = 0;
        public const int MAX_ZOOM_LEVEL = 20;

        public const int SRID = 4326;

        public const double MAX_RADIUS = 41_000_000;
    }
}
