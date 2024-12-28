using Microsoft.AspNetCore.Identity;
using MTAA_Backend.Domain.Entities.Chats;
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

        public ICollection<Chat> Chats { get; set; } = new HashSet<Chat>();
        public ICollection<BaseMessage> Messages { get; set; } = new HashSet<BaseMessage>();

        public MyImageGroup Avatar { get; set; }
        public Guid AvatarId { get; set; }
    }
}