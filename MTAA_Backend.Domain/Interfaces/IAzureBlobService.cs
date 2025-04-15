using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces
{
    public interface IAzureBlobService
    {
        public void Initialize(string connectionString, string containerName);
        public Task<BlobContentInfo> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);
        public Task<ICollection<BlobContentInfo>> UploadFilesAsync(ICollection<IFormFile> files, CancellationToken cancellationToken = default);
        public Task<Azure.Response<bool>?> RemoveFileAsync(string fileName, CancellationToken cancellationToken = default);
    }
}
