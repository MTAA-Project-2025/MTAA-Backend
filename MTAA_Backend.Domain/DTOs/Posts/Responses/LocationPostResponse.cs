using MTAA_Backend.Domain.DTOs.Locations.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Posts.Responses
{
    public class LocationPostResponse : SimplePostResponse
    {
        public Guid? LocationId { get; set; }
        public DateTime EventTime { get; set; }
        public SimpleLocationPointResponse Point { get; set; }
        public string Description { get; set; }
        public string OwnerDisplayName { get; set; }
    }
}
