using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Files
{
    public class MyFile : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public long Length { get; set; }

        public string ShortPath { get; set; }
        public string FullPath { get; set; }

        public string FileType { get; set; }

        public string Name { get; set; }

        public FileMessage? FileMessage { get; set; }
        public Guid? FileMessageId { get; set; }

        public VoiceMessage? VoiceMessage { get; set; }
        public Guid? VoiceMessageId { get; set; }

        public GifMessage? GifMessage { get; set; }
        public Guid? GifMessageId { get; set; }
    }
}
