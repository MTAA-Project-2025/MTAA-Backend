using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Resources.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Groups.BaseGroups.Responses
{
    public class SimpleBaseGroupResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }
        public string Visibility { get; set; } = GroupVisibilityTypes.Invisible;

        public MyImageGroupResponse Image { get; set; }
    }
}
