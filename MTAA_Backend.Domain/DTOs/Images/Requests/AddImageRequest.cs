using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Images.Requests
{
    public class AddImageRequest
    {
        public IFormFile Image { get; set; }
        public int Position { get; set; }
    }
}
