using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Images
{
    public class UserPresetAvatarImage : MyImageGroup
    {
        public ICollection<UserAvatar> UserAvatars { get; set; } = new HashSet<UserAvatar>();
    }
}
