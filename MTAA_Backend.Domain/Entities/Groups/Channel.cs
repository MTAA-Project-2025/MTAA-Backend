using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Resources.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Groups
{
    public class Channel : BaseGroup
    {
        public string DisplayName { get; set; }
        public string IdentificationName { get; set; }
        public string Description { get; set; }

        public MyImageGroup? Image { get; set; }
        public Guid? ImageId { get; set; }

        public User Owner { get; set; }
        public string OwnerId { get; set; }

        public Channel() : base(GroupTypes.Chat)
        {
        }
    }
}
