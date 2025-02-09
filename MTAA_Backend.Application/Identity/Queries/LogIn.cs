using MediatR;
using Microsoft.Identity.Client;
using MTAA_Backend.Domain.DTOs.Users.Identity.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Identity.Queries
{
    public class LogIn : IRequest<TokenDTO>
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
