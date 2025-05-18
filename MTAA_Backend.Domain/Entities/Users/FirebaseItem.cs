using MTAA_Backend.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Users
{
    public class FirebaseItem : BaseEntity
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
