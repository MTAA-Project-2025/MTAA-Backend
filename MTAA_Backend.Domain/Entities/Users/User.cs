using Microsoft.AspNetCore.Identity;
using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Users
{
    public class User : IdentityUser, IAuditable
    {
        public string DisplayName { get; set; } = "";
        public DateTime BirthDate { get; set; }

        public string Status { get; set; }

        public DateTime LastSeen { get; set; } = DateTime.UtcNow;

        public DateTime DataCreationTime { get; set; } = DateTime.UtcNow;
        public DateTime? DataLastDeleteTime { get; set; }
        public DateTime? DataLastEditTime { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsEdited { get; set; }

        public ICollection<UserContact> Contacts { get; set; } = new HashSet<UserContact>();
        public ICollection<UserContact> ContactOf { get; set; } = new HashSet<UserContact>();

        public ICollection<BaseGroup> Groups { get; set; } = new HashSet<BaseGroup>();
        public ICollection<BaseMessage> Messages { get; set; } = new HashSet<BaseMessage>();
        public ICollection<UserGroupMembership> UserGroupMemberships { get; set; } = new HashSet<UserGroupMembership>();

        public UserAvatar? Avatar { get; set; }
        public Guid? AvatarId { get; set; }

        public ICollection<Channel> OwnedChannels { get; set; } = new HashSet<Channel>();
        public ICollection<ContactChat> OwnedChats { get; set; } = new HashSet<ContactChat>();
    }
}