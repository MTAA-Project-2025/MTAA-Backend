using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.CQRS.Users.Identity.Commands
{
    public class SignUpVerifyEmail : IRequest<bool>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
