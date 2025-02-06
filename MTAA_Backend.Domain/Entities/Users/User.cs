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
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

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

        public MyImageGroup? Avatar { get; set; }
        public Guid? AvatarId { get; set; }
    }
}