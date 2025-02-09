using MTAA_Backend.Domain.Entities.Shared;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Images
{
    public class MyImageGroup : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }

        public ICollection<MyImage> Images { get; set; } = new HashSet<MyImage>();

        public UserAvatar? UserAvatar { get; set; }
        public string? UserAvatarId { get; set; }
    }
}
