using MTAA_Backend.Domain.Resources.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Groups
{
    public class ContactChat : BaseGroup
    {
        public ContactChat() : base(GroupTypes.Chat)
        {
        }
    }
}
