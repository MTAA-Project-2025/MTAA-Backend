using MTAA_Backend.Domain.Entities.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Users
{
    public class UserAvatar
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public MyImageGroup? CustomAvatar { get; set; }
        public Guid? CustomAvatarId { get; set; }

        public UserPresetAvatarImage? PresetAvatar { get; set; }
        public Guid? PresetAvatarId { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
