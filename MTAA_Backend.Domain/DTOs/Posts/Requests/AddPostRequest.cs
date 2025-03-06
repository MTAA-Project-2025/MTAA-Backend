using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Posts.Requests
{
    public class AddPostRequest
    {
        public ICollection<IFormFile> Images { get; set; }
        public string Description { get; set; }
        public AddPostRequest? Location { get; set; }
    }
}
