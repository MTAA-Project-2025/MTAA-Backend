using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Other
{
    public struct TimeConstants
    {
        public readonly static TimeSpan VerificationSpan = TimeSpan.FromMinutes(5);
        public readonly static TimeSpan DefaultCacheExpirationTime = TimeSpan.FromHours(1);
    }
}
