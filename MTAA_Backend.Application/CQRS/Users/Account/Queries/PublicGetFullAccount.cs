using MediatR;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.CQRS.Users.Account.Queries
{
    public class PublicGetFullAccount : IRequest<PublicFullAccountResponse>
    {
        public string UserId { get; set; }
    }
}
