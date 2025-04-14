using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Shared.Requests
{
    public class GlobalSearchRequest
    {
        public string FilterStr { get; set; } = "";
        public PageParameters PageParameters { get; set; }
    }
}
