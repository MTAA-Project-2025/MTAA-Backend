using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Account.Commands
{
    public class UpdateAccountUsername : IRequest
    {
        public string Username { get; set; }
    }
}
