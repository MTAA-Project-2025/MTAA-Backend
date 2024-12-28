using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Users.Requests
{
    public class StartSignUpEmailVerificationRequest
    {
        public string Email { get; set; }
    }
}
