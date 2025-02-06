using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Groups
{
    public struct GroupTypes
    {
        public const string Chat = "Chat";
        public const string Channel = "Channel";

        public static List<string> GetAll()
        {
            return new List<string>
            {
                Chat,
                Channel
            };
        }
    }
}
