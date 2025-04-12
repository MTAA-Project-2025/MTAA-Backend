using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Locations.Requests
{
    public class GetLocationPointsRequest
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int ZoomLevel { get; set; }
        public double Radius { get; set; }
    }
}
