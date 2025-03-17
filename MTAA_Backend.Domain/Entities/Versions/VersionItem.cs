using MTAA_Backend.Domain.Entities.Users;
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
        [Key]
        public VersionItemType Id { get; set; }
        public int Version { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
