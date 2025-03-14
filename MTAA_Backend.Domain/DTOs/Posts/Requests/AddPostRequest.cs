using Microsoft.AspNetCore.Http;
using MTAA_Backend.Domain.DTOs.Images.Requests;
using MTAA_Backend.Domain.DTOs.Locations.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Posts.Requests
{
    public class AddPostRequest
    {
        public ICollection<AddImageRequest> Images { get; set; }
        public string Description { get; set; }
        public AddLocationRequest? Location { get; set; }
    }
}
