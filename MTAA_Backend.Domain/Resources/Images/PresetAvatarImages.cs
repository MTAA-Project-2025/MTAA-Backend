using MTAA_Backend.Domain.Entities.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Images
{
    public struct PresetAvatarImages
    {
        public static Guid Image1Id = Guid.Parse("416c7d33-0a25-4176-b783-64b25919ac12");
        public static Guid Image2Id = Guid.Parse("161750a4-9b50-4a1c-a5f1-3221640533c6");
        public static Guid Image3Id = Guid.Parse("3e4f4c14-f4ae-4238-95b1-075d1e8a9981");
        public static Guid Image4Id = Guid.Parse("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed");
        public static Guid Image5Id = Guid.Parse("9ad61bee-053b-4042-8b4a-860fe80dd05a");
        public static Guid Image6Id = Guid.Parse("d1a56d08-a7de-4855-8a13-5fbda2ca4843");

        public static List<Guid> GetAll()
        {
            return new List<Guid>()
            {
                Image1Id,
                Image2Id,
                Image3Id,
                Image4Id,
                Image5Id,
                Image6Id,
            };
        }
    }
}