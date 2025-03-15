using MTAA_Backend.Domain.DTOs.Images.Requests;
using MTAA_Backend.Domain.DTOs.Locations.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Posts.Requests
{
    public class UpdatePostRequest
    {
        public Guid Id { get; set; }
        public ICollection<UpdateImageRequest> Images { get; set; }
        public string Description { get; set; }
        public UpdateLocationRequest? Location { get; set; }
    }
}
