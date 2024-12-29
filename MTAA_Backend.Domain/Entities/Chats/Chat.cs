using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Entities.Shared;
using MTAA_Backend.Domain.Entities.Messages;

namespace MTAA_Backend.Domain.Entities.Chats
{
    public class Chat : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }
        public string Type { get; set; }
        public string Theme { get; set; }

        public ICollection<User> Participants { get; set; } = new HashSet<User>();
        public ICollection<BaseMessage> Messages { get; set; } = new HashSet<BaseMessage>();
    }
}