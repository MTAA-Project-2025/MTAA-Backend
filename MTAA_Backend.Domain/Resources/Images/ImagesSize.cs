using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Images
{
    public class ImagesSize
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public ImageSizeType Type { get; set; }
    }
}
