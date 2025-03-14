using MTAA_Backend.Domain.DTOs.Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Posts.Requests
{
    public class GetGlobalPostsRequest
    {
        public string FilterStr { get; set; } = "";
        public PageParameters PageParameters { get; set; }
    }
}
