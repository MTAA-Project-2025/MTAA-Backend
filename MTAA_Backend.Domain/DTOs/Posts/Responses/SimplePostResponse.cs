using MTAA_Backend.Domain.DTOs.Images.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Posts.Responses
{
    public class SimplePostResponse
    {
        public Guid Id { get; set; }
        public MyImageResponse SmallFirstImage { get; set; }
        public DateTime DataCreationTime { get; set; }
    }
}
