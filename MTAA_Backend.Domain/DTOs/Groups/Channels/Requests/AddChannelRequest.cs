using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Groups.Channels.Requests
{
    public class AddChannelRequest
    {
        public IFormFile? Image { get; set; }
        public string Visibility { get; set; }
        public string DisplayName { get; set; } = "";
        public string IdentificationName { get; set; }
        public string Description { get; set; } = "";
    }
}
