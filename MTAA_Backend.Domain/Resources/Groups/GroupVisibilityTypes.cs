using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Groups
{
    public struct GroupVisibilityTypes
    {
        public const string Invisible = "Invisible";
        public const string Private = "Private";
        public const string WithRequest = "WithRequest";
        public const string Public = "Public";

        public static List<string> GetAll()
        {
            return new List<string>
            {
                Invisible,
                Private,
                WithRequest,
                Public
            };
        }


        public static List<string> GetAllPublic()
        {
            return new List<string>
            {
                Private,
                WithRequest,
                Public
            };
        }
    }
}
