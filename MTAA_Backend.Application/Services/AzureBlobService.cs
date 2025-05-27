using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using MTAA_Backend.Domain.Interfaces;

namespace MTAA_Backend.Application.Services
{
    /// <summary>
    /// Provides services for interacting with Azure Blob Storage.
    /// </summary>
    public class AzureBlobService : IAzureBlobService
    {
        private BlobServiceClient blobServiceClient;
        private BlobContainerClient blobContainerClient;

        /// <summary>
        /// Initializes the Azure Blob Service with the specified connection string and container name.
        /// </summary>
        /// <param name="connectionString">The Azure Blob Storage connection string.</param>
        /// <param name="containerName">The name of the blob container.</param>
        public void Initialize(string connectionString, string containerName)
        {
            blobServiceClient = new BlobServiceClient(connectionString);
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

        /// <summary>
        /// Uploads a single file to Azure Blob Storage.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning the blob content info.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the service is not initialized.</exception>
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

        /// <summary>
        /// Uploads multiple files to Azure Blob Storage.
        /// </summary>
        /// <param name="files">The collection of files to upload.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning a collection of blob content info.</returns>
        public async Task<ICollection<BlobContentInfo>> UploadFilesAsync(ICollection<IFormFile> files, CancellationToken cancellationToken = default)
        {
            var azureResponses = new List<BlobContentInfo>(files.Count);
            foreach(var file in files)
            {
                azureResponses.Add(await UploadFileAsync(file, cancellationToken));
            }
            return azureResponses;
        }

        /// <summary>
        /// Removes a file from Azure Blob Storage if it exists.
        /// </summary>
        /// <param name="fileName">The name of the file to remove.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning the response indicating if the file was deleted.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the service is not initialized.</exception>
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
