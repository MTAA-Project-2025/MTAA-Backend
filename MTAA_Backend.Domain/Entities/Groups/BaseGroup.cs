using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Entities.Shared;
using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Resources.Groups;

namespace MTAA_Backend.Domain.Entities.Groups
{
    public abstract class BaseGroup : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Type { get; set; }

        public ICollection<User> Participants { get; set; } = new HashSet<User>();
        public ICollection<BaseMessage> Messages { get; set; } = new HashSet<BaseMessage>();

        public ICollection<UserGroupMembership> UserGroupMemberships = new HashSet<UserGroupMembership>();

        public string Visibility { get; set; } = GroupVisibilityTypes.Invisible;

        public BaseGroup(string Type)
        {
            this.Type = Type;
        }
    }
}