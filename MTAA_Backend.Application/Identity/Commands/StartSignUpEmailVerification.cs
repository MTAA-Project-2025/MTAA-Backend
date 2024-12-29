using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Identity.Commands
{
    public class StartSignUpEmailVerification : IRequest
    {
        public string Email { get; set; }
    }
}
