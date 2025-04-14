using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Locations.Responses
{
    public class PostLocationResponse
    {
        public Guid Id { get; set; }
        public DateTime EventTime { get; set; }
        public SimpleLocationPointResponse Point { get; set; }
    }
}
