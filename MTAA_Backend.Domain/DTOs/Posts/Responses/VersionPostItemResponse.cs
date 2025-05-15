using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Posts.Responses
{
    public class VersionPostItemResponse
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
