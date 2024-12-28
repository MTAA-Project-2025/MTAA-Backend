using MTAA_Backend.Domain.Entities.Chats;
using MTAA_Backend.Domain.Entities.Shared;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Messages
{
    public class BaseMessage : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Type { get; set; }

        public bool IsRead { get; set; }

        public Chat Chat { get; set; }
        public Guid ChatId { get; set; }

        public User Sender { get; set; }
        public string SenderId { get; set; }
    }
}
