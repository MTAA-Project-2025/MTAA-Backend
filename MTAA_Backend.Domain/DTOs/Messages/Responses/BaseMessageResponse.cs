using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Entities.Shared;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Messages.Responses
{
    public class BaseMessageResponse : IAuditable
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }

        public DateTime DataCreationTime { get; set; }
        public DateTime? DataLastDeleteTime { get; set; }
        public DateTime? DataLastEditTime { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsEdited { get; set; }
    }
}
