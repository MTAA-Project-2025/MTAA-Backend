using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Locations.Requests
{
    public class AddLocationRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DateTime EventTime { get; set; }
    }
}
