using MTAA_Backend.Domain.Entities.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Messages
{
    public class Message : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }

        public string Type { get; set; }
        public string? Content { get; set; }
        public string? Url { get; set; }

        public bool IsRead { get; set; } = false;
    }
}
