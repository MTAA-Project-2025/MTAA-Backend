using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Images.Requests
{
    public class UpdateImageRequest
    {
        public IFormFile? NewImage { get; set; }
        public Guid? OldImageId { get; set; }
        public int Position { get; set; }
    }
}
