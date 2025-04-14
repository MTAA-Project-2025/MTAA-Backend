using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.CQRS.Users.Account.Commands
{
    public class UpdateAccountBirthDate : IRequest
    {
        public DateTime BirthDate { get; set; }
    }
}
