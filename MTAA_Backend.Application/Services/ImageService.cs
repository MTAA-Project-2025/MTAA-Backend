using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Images.Requests;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly IStringLocalizer<ErrorMessages> _localizer;
        private const int QUALITY = 50;
        private readonly string url;

        public ImageService(IAzureBlobService azureBlobService,
                            IStringLocalizer<ErrorMessages> localizer,
                            IConfiguration configuration)
        {
            _azureBlobService = azureBlobService;
            _localizer = localizer;

            var filesConfig = configuration.GetSection("AzureStorage");
            var connectionString = filesConfig["ConnectionString"];
            var containerName = filesConfig["ImagesContainerName"];
            url = filesConfig["StorageUrl"];
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName))
            {
                throw new Exception("AzureBlobService not initialized");
            }

            _azureBlobService.Initialize(connectionString, containerName);
        }

        public async Task<MyImageGroup> SaveImage(IFormFile file, int position, ImageSavingTypes type, CancellationToken cancellationToken = default)
        {
            return await SaveImage(file, ImagesSizes.Sizes[type], position, cancellationToken);
        }
        public async Task<ICollection<MyImageGroup>> SaveImages(ICollection<AddImageRequest> requests, ImageSavingTypes type, CancellationToken cancellationToken = default)
        {
            return await SaveImages(requests, ImagesSizes.Sizes[type], cancellationToken);
        }


        private async Task<ICollection<MyImageGroup>> SaveImages(ICollection<AddImageRequest> requests, ICollection<ImagesSize> sizes, CancellationToken cancellationToken = default)
        {
            ICollection<MyImageGroup> images = new List<MyImageGroup>(requests.Count());
            foreach (var request in requests)
            {
                images.Add(await SaveImage(request.Image, sizes, request.Position, cancellationToken));
            }
            return images;
        }

        private async Task<MyImageGroup> SaveImage(IFormFile file, ICollection<ImagesSize> sizes, int position, CancellationToken cancellationToken = default)
        {
            var ext = Path.GetExtension(file.FileName);
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            if (!allowedExtensions.Contains(ext))
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.ImageFormatNotAllowed], HttpStatusCode.BadRequest);
            }

            var myImageGroup = new MyImageGroup()
            {
                Title = file.FileName,
                Position = position
            };
            string uniqueString = myImageGroup.Id.ToString();

            using var image = file.ConvertToImageSharp();

            var images = await SaveImageWithSizes(image, sizes, uniqueString, cancellationToken);
            foreach(var myImg in images)
            {
                myImageGroup.Images.Add(myImg);
                myImg.ImageGroup = myImageGroup;
            }

            return myImageGroup;
        }

        private async Task<ICollection<MyImage>> SaveImageWithSizes(Image image, ICollection<ImagesSize> sizes, string uniqueString, CancellationToken cancellationToken = default)
        {
            var aspectRatio = (double)image.Width / image.Height;

            var images = new List<MyImage>(sizes.Count);
            foreach (var size in sizes)
            {
                int width = 0, height = 0;
                int x = 0, y = 0;
                int? sizeWidth = size.Width;
                int? sizeHeight = size.Height;
                int cropWidth = 0;

                if (sizeWidth != null && (int)sizeWidth > image.Width) sizeWidth = image.Width;
                if (sizeHeight != null && (int)sizeHeight > image.Height) sizeHeight = image.Height;

                if (sizeWidth != null && sizeHeight != null)
                {
                    if (aspectRatio > 1)
                    {
                        width = (int)(sizeHeight * aspectRatio);
                        height = (int)sizeHeight;
                        x = (width - height) / 2;
                        cropWidth = height;
                    }
                    else if (aspectRatio < 1)
                    {
                        width = (int)sizeWidth;
                        height = (int)(sizeWidth / aspectRatio);
                        y = (height - width) / 2;
                        cropWidth = width;
                    }
                    else
                    {
                        width = (int)sizeWidth;
                        height = (int)sizeHeight;
                    }
                }
                else if (sizeWidth != null)
                {
                    width = (int)sizeWidth;
                    height = (int)(sizeWidth / aspectRatio);
                }
                else if (sizeHeight != null)
                {
                    width = (int)(sizeHeight * aspectRatio);
                    height = (int)sizeHeight;
                }

                image.Mutate(e => e.Resize(width, height));
                if (x != 0 || y != 0)
                {
                    image.Mutate(e => e.Crop(new Rectangle(x, y, cropWidth, cropWidth)));
                }

                var newFileName = uniqueString + "_" + size.Type + "." + ImagesFileTypes.Jpg;
                var file = image.ConvertToIFormFile(newFileName, new JpegEncoder()
                {
                    Quality = QUALITY
                });
                var result = await _azureBlobService.UploadFileAsync(file, cancellationToken);

                var myImage = new MyImage()
                {
                    AspectRatio = aspectRatio,
                    FileType = ImagesFileTypes.Jpg,
                    FullPath = url + newFileName,
                    Height = height,
                    Width = width,
                    Type = size.Type,
                    ShortPath = uniqueString + "_" + size.Type
                };
                images.Add(myImage);
            }
            return images;
        }

        public async Task RemoveImageGroup(MyImageGroup myImageGroup, CancellationToken cancellationToken = default)
        {
            if (myImageGroup == null) return;

            foreach (var image in myImageGroup.Images)
            {
                var name = image.ShortPath + "." + image.FileType;
                await _azureBlobService.RemoveFileAsync(name, cancellationToken);
            }
        }

        public bool IsImagesHaveSameAspectRatio(ICollection<IFormFile> files)
        {
            double standardAspectRatio = -1;
            foreach (var file in files)
            {
                var aspectRatio = GetImageAspectRatio(file);
                if (aspectRatio <= 0)
                {
                    throw new HttpException(_localizer[ErrorMessagesPatterns.ImageFormatNotAllowed], HttpStatusCode.BadRequest);
                }
                if (standardAspectRatio == -1)
                {
                    standardAspectRatio = aspectRatio;
                    break;
                }
                if (Math.Abs(standardAspectRatio - aspectRatio) >= 0.001) return false;
            }
            return true;
        }
        public double GetImageAspectRatio(IFormFile file)
        {
            try
            {
                var image = file.ConvertToImageSharp();
                return image.Width / image.Height;
            }
            catch
            {
                return -1;
            }
        }
    }
}
