using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Users.Account.Responses
{
    public class UserFullAccountResponse : PublicFullAccountResponse
    {
        public DateTime? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int LikesCount { get; set; }
    }
}
