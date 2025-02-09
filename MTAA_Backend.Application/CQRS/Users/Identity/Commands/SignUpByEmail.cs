using MediatR;
using MTAA_Backend.Domain.DTOs.Users.Identity.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.CQRS.Users.Identity.Commands
{
    public class SignUpByEmail : IRequest<TokenDTO>
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
