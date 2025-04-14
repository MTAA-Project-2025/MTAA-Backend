using MTAA_Backend.Domain.Resources.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Versioning.Responses
{
    public class VersionItemResponse
    {
        public VersionItemType Type { get; set; }
        public int Version { get; set; }
    }
}
