using Microsoft.AspNetCore.Http;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Resources.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces
{
    public interface IImageService
    {
        public Task<MyImageGroup> SaveImage(IFormFile file, ImageSavingTypes type, CancellationToken cancellationToken = default);
        public Task<ICollection<MyImageGroup>> SaveImages(ICollection<IFormFile> files, ImageSavingTypes type, CancellationToken cancellationToken = default);

        public Task RemoveImageGroup(MyImageGroup myImageGroup, CancellationToken cancellationToken = default);

        public bool IsImagesHaveSameAspectRatio(ICollection<IFormFile> files);
        public double GetImageAspectRatio(IFormFile file);
    }
}
