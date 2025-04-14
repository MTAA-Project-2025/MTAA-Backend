using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Groups
{
    public class UserGroupMembership
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public User User { get; set; }
        public string UserId { get; set; }
        
        public BaseGroup Group { get; set; }
        public Guid GroupId { get; set; }

        public bool IsNotificationEnabled { get; set; } = true;
        public bool IsArchived { get; set; } = false;

        public BaseMessage? LastMessage { get; set; }
        public Guid? LastMessageId { get; set; }

        public int UnreadMessagesCount { get; set; } = 0;
    }
}
