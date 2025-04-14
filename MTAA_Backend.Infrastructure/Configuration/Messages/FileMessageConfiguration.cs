using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Messages
{
    public class FileMessageConfiguration : IEntityTypeConfiguration<FileMessage>
    {
        public void Configure(EntityTypeBuilder<FileMessage> builder)
        {
            builder.HasOne(e => e.File)
                   .WithOne(e => e.FileMessage)
                   .HasForeignKey<FileMessage>(e => e.FileId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
