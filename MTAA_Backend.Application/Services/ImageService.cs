using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.Extensions;
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

        public async Task<MyImageGroup> SaveImage(IFormFile file, ImageSavingTypes type, CancellationToken cancellationToken = default)
        {
            switch (type)
            {
                case ImageSavingTypes.UserAvatar:
                    return await SaveImage(file, new int[] { 100, 300 }, cancellationToken);
                    break;
            }
            return null;
        }
        public async Task<ICollection<MyImageGroup>> SaveImages(ICollection<IFormFile> files, ImageSavingTypes type, CancellationToken cancellationToken = default)
        {
            switch (type)
            {
                case ImageSavingTypes.UserAvatar:
                    return await SaveImages(files, new int[] { 100, 300 }, cancellationToken);
                    break;
            }
            return new List<MyImageGroup>();
        }


        private async Task<ICollection<MyImageGroup>> SaveImages(ICollection<IFormFile> files, int[] sizes, CancellationToken cancellationToken = default)
        {
            ICollection<MyImageGroup> images = new List<MyImageGroup>(files.Count());
            foreach (var file in files)
            {
                images.Add(await SaveImage(file, sizes, cancellationToken));
            }
            return images;
        }

        private async Task<MyImageGroup> SaveImage(IFormFile file, int[] sizes, CancellationToken cancellationToken = default)
        {
            var ext = Path.GetExtension(file.FileName);
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            if (!allowedExtensions.Contains(ext))
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.ImageFormatNotAllowed], HttpStatusCode.BadRequest);
            }

            var myImageGroup = new MyImageGroup()
            {
                Title = file.FileName
            };
            string uniqueString = myImageGroup.Id.ToString();
            var newFileName = uniqueString + "."+ ImagesFileTypes.Jpg;

            using var image = file.ConvertToImageSharp();

            var images = await SaveImageWithSizes(image, sizes, newFileName, cancellationToken);
            foreach(var myImg in images)
            {
                myImageGroup.Images.Add(myImg);
                myImg.ImageGroup = myImageGroup;
            }

            return myImageGroup;
        }

        private async Task<ICollection<MyImage>> SaveImageWithSizes(Image image, int[] sizes, string uniqueString, CancellationToken cancellationToken = default)
        {
            var aspectRatio = (double)image.Width / image.Height;

            var images = new List<MyImage>(sizes.Length);
            foreach (var size in sizes)
            {
                int width, height;

                if (aspectRatio > 1)
                {
                    //horizontal image
                    width = size;
                    height = (int)(size / aspectRatio);
                }
                else if (aspectRatio < 1)
                {
                    //vertical image
                    width = (int)(size * aspectRatio);
                    height = size;
                }
                else
                {
                    //square image
                    width = size;
                    height = size;
                }

                var newWidth = width;
                var newHeight = height;
                if (width > image.Width) newWidth = image.Width;
                if (height > image.Height) newHeight = image.Height;

                image.Mutate(x => x.Resize(newWidth, newHeight));

                var newFileName = uniqueString + "_" + size + "." + ImagesFileTypes.Jpg;
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
                    Height = newHeight,
                    Width = newWidth,
                    ShortPath = uniqueString + "_" + size
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
    }
}
