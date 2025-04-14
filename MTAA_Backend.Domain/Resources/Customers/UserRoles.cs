using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Customers
{
    public struct UserRoles
    {
        public const string Visitor = "Visitor";
        public const string User = "User";
        public const string Moderator = "Moderator";

        public static List<string> GetAllRoles()
        {
            return new List<string>()
            {
                Visitor,
                User,
                Moderator
            };
        }
    }
}
