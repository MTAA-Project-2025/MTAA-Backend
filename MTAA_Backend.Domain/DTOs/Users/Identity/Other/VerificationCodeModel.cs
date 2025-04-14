using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Users.Identity.Other
{
    public class VerificationCodeModel
    {
        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        public bool IsVerified { get; set; }
    }
}
