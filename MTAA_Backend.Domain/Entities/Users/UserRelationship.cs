using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Users
{
    public class UserRelationship
    {
        public User User1 { get; set; }
        public User User2 { get; set; }

        public string User1Id { get; set; }
        public string User2Id { get; set; }

        public bool IsUser1Followig {  get; set; }
        public bool IsUser2Followig { get; set; }
    }
}
