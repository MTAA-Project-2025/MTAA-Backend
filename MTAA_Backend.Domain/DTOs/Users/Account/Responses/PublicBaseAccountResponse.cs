using MTAA_Backend.Domain.DTOs.Images.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Users.Account.Responses
{
    public class PublicBaseAccountResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public MyImageGroupResponse? Avatar { get; set; }
        public bool IsFollowing { get; set; }
    }
}
