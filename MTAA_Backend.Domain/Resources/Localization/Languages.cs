using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Localization
{
    public struct Languages
    {
        public const string SK = "sk-SK";
        public const string EN = "en-US";

        public static List<string> GetAll()
        {
            return new List<string>
            {
                SK,
                EN
            };
        }
    }
}
