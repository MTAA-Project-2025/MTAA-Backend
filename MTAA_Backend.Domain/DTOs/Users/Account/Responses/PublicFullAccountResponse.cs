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
    public class PublicFullAccountResponse : PublicBaseAccountResponse
    {
        public DateTime DataCreationTime { get; set; }

        public int FriendsCount { get; set; }
        public int FollowersCount { get; set; }
    }
}
