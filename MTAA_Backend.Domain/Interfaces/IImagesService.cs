using Microsoft.AspNetCore.Http;
using MTAA_Backend.Domain.DTOs.Images.Requests;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces
{
    public interface IImageService
    {
        public Task<MyImageGroup> SaveImage(IFormFile file, int position, ImageSavingTypes type, CancellationToken cancellationToken = default);
        public Task<ICollection<MyImageGroup>> SaveImages(ICollection<AddImageRequest> requests, ImageSavingTypes type, CancellationToken cancellationToken = default);

        public Task RemoveImageGroup(MyImageGroup myImageGroup, CancellationToken cancellationToken = default);

        public bool IsImagesHaveSameAspectRatio(ICollection<IFormFile> files);
        public double GetImageAspectRatio(IFormFile file);
    }
}
