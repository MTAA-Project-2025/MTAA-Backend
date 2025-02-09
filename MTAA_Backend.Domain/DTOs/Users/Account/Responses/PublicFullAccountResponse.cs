using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Users.Account.Responses
{
    public class PublicFullAccountResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public DateTime BirthDate { get; set; }

        public string Status { get; set; }

        public DateTime LastSeen { get; set; }

        public DateTime DataCreationTime { get; set; }

        public bool IsContact { get; set; }
        public bool IsBlocked { get; set; }

        public MyImageGroupResponse? Avatar { get; set; }
    }
}
