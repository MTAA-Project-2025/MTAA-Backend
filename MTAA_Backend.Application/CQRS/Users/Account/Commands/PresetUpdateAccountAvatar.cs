using MediatR;
using MTAA_Backend.Domain.DTOs.Images.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.CQRS.Users.Account.Commands
{
    public class PresetUpdateAccountAvatar : IRequest<MyImageGroupResponse>
    {
        public string ImageGroupId { get; set; }
    }
}
