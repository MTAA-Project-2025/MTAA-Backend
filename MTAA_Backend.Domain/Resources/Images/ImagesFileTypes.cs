using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Images
{
    public struct ImagesFileTypes
    {
        public const string Jpg = "jpg";
        public const string Png = "png";
        public const string Gif = "gif";
        public const string Bmp = "bmp";

        public static List<string> GetAll()
        {
            return new List<string>()
            {
                Jpg,
                Png,
                Gif,
                Bmp
            };
        }
    }
}
