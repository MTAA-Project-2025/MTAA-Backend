using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.shared
{
    public class BaseEntity : IAuditable
    {
        public DateTime DataCreationTime { get; set; } = DateTime.UtcNow;
        public DateTime? DataDeleteTime { get; set; }
        public DateTime? DataLastEditTime { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsEdited { get; set; }
    }
}
