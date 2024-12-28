using MTAA_Backend.Domain.Entities.Chats;
using MTAA_Backend.Domain.Entities.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Users
{
    public class User : IdentityUser, IAuditable
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string Status { get; set; }

        public DateTime LastSeen { get; set; }

        public DateTime DataCreationTime { get; set; } = DateTime.UtcNow;
        public DateTime? DataLastDeleteTime { get; set; }
        public DateTime? DataLastEditTime { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsEdited { get; set; }

        public ICollection<User> Contacts { get; set; } = new HashSet<User>();
        public ICollection<Chat> Chats { get; set; } = new HashSet<Chat>();
    }
}