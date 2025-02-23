using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Files.Responses
{
    public class MyFileResponse
    {
        public Guid Id { get; set; }
        public long Length { get; set; }
        public string ShortPath { get; set; }
        public string FullPath { get; set; }
        public string FileType { get; set; }
        public string Name { get; set; }
    }
}
