using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Users.Identity.Requests
{
    public class SignUpVerifyEmailRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
