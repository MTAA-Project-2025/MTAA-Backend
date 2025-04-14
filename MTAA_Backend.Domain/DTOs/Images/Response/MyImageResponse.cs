using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Resources.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Images.Response
{
    public class MyImageResponse
    {
        public Guid Id { get; set; }

        public string ShortPath { get; set; }
        public string FullPath { get; set; }

        public string FileType { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }
        public ImageSizeType Type { get; set; }

        public double AspectRatio { get; set; }
    }
}
