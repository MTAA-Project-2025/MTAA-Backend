using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Files
{
    public class MyFileConfiguration : IEntityTypeConfiguration<MyFile>
    {
        public void Configure(EntityTypeBuilder<MyFile> builder)
        {
            builder.HasOne(e => e.FileMessage)
                   .WithOne(e => e.File)
                   .HasForeignKey<MyFile>(e => e.FileMessageId)
                   .IsRequired(false);

            builder.HasOne(e => e.VoiceMessage)
                   .WithOne(e => e.File)
                   .HasForeignKey<MyFile>(e => e.VoiceMessageId)
                   .IsRequired(false);

            builder.HasOne(e => e.GifMessage)
                   .WithOne(e => e.File)
                   .HasForeignKey<MyFile>(e => e.GifMessageId)
                   .IsRequired(false);
        }
    }
}
