using MTAA_Backend.Domain.Entities.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Users
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string Status { get; set; }

        public DateTime LastSeen { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<User> Contacts { get; set; } = new List<User>();
        public List<Chat> Chats { get; set; } = new List<Chat>();
    }
}