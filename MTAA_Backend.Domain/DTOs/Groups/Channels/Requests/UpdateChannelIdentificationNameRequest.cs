using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Groups.Channels.Requests
{
    public class UpdateChannelIdentificationNameRequest
    {
        public Guid Id { get; set; }
        public string IdentificationName { get; set; }
    }
}
