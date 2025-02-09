using MediatR;
using Microsoft.AspNetCore.Http;
using MTAA_Backend.Domain.DTOs.Images.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Account.Commands
{
    public class CustomUpdateAccountAvatar : IRequest<MyImageGroupResponse>
    {
        public IFormFile Avatar { get; set; }
    }
}
