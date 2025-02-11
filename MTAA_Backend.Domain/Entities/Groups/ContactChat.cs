using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Resources.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Groups
{
    public class ContactChat : BaseGroup
    {
        public string IdentificationName { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public User Contact { get; set; }
        public string ContactId { get; set; }

        public ContactChat() : base(GroupTypes.Chat)
        {
        }
    }
}
