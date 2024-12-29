using MTAA_Backend.Domain.Entities.Shared;

namespace MTAA_Backend.Domain.Entities.Users
{
    public class UserContact : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public User User { get; set; }
        public string UserId { get; set; }

        public User Contact { get; set; }
        public string ContactId { get; set; }

        public bool IsBlocked { get; set; }
        public string ContactType { get; set; }
    }
}
