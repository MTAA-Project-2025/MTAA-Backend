using MTAA_Backend.Domain.Entities.Groups;
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

        public BaseGroup Group { get; set; }
        public Guid GroupId { get; set; }

        public User Sender { get; set; }
        public string SenderId { get; set; }

        public ICollection<UserGroupMembership> LastMessageUserGroupMemberships { get; set; } = new HashSet<UserGroupMembership>();
    }
}
