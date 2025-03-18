using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Resources.Versioning;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Versions
{
    public class VersionItem
    {
        public VersionItemType Type { get; set; }
        public int Version { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
