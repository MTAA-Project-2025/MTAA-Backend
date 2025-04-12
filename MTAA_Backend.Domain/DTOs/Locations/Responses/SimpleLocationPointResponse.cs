using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Resources.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Locations.Responses
{
    public class SimpleLocationPointResponse
    {
        public Guid Id { get; set; }
        public Guid? PostId { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public LocationPointType Type { get; set; }
        public int ZoomLevel { get; set; }

        public int ChildCount { get; set; }
        public MyImageResponse? Image { get; set; }
    }
}
