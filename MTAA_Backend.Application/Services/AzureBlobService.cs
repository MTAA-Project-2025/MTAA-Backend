using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using MTAA_Backend.Domain.Interfaces;

namespace MTAA_Backend.Application.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private BlobServiceClient blobServiceClient;
        private BlobContainerClient blobContainerClient;
        public void Initialize(string connectionString, string containerName)
        {
            blobServiceClient = new BlobServiceClient(connectionString);
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<BlobContentInfo> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (blobContainerClient == null)
            {
                throw new InvalidOperationException("AzureBlobService not initialized");
            }
            var fileName = file.FileName;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                var response = await blobContainerClient.UploadBlobAsync(fileName, memoryStream, cancellationToken);

                return response;
            }
        }

        public async Task<ICollection<BlobContentInfo>> UploadFilesAsync(ICollection<IFormFile> files, CancellationToken cancellationToken = default)
        {
            var azureResponses = new List<BlobContentInfo>(files.Count);
            foreach(var file in files)
            {
                azureResponses.Add(await UploadFileAsync(file, cancellationToken));
            }
            return azureResponses;
        }


        public async Task<Azure.Response<bool>?> RemoveFileAsync(string fileName, CancellationToken cancellationToken = default)
        {
            if (blobContainerClient == null)
            {
                throw new InvalidOperationException("AzureBlobService not initialized");
            }

            var response = await blobContainerClient.DeleteBlobIfExistsAsync(fileName, cancellationToken: cancellationToken);
            return response;
        }
    }
}
