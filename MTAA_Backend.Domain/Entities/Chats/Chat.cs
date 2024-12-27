using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Chats
{
    public class Chat
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }
        public string Type { get; set; }
        public string Theme { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<User> Participants { get; set; } = new List<User>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}