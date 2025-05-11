using MTAA_Backend.Domain.DTOs.Locations.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Posts.Responses
{
    public class SchedulePostResponse : SimplePostResponse
    {
        public bool IsHidden { get; set; }
        public DateTime? SchedulePublishDate { get; set; }
        public string? HiddenReason { get; set; }

        public string Description { get; set; }
        public int Version { get; set; }
    }
}
