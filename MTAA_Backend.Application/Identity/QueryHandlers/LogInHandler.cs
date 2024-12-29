using MediatR;
using MTAA_Backend.Application.Identity.Queries;
using MTAA_Backend.Domain.DTOs.Users.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Identity.QueryHandlers
{
    public class LogInHandler : IRequestHandler<LogIn, TokenDTO>
    {
        public Task<TokenDTO> Handle(LogIn request, CancellationToken cancellationToken)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
