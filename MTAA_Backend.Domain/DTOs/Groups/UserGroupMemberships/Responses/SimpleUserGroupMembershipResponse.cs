using MTAA_Backend.Domain.DTOs.Groups.BaseGroups.Responses;
using MTAA_Backend.Domain.DTOs.Messages.Responses;
using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Groups.UserGroupMemberships.Responses
{
    public class SimpleUserGroupMembershipResponse
    {
        public Guid Id { get; set; }

        public SimpleBaseGroupResponse Group { get; set; }

        public bool IsNotificationEnabled { get; set; }
        public bool IsArchived { get; set; }

        public BaseMessageResponse? LastMessage { get; set; }

        public int UnreadMessagesCount { get; set; }
    }
}
