using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats;

namespace MTAA_Backend.Application.Extensions
{
    public static class ImageSharpExtensions
    {
        public static Image<Rgba32> ConvertToImageSharp(this IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                throw new ArgumentException("Invalid form file");
            }

            using var stream = formFile.OpenReadStream();
            return Image.Load<Rgba32>(stream);
        }
        public static IFormFile ConvertToIFormFile(this Image image, string fileName, IImageEncoder encoder)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, encoder);
            memoryStream.Position = 0;

            return new FormFile(memoryStream, 0, memoryStream.Length, "image", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };
        }
    }
}
