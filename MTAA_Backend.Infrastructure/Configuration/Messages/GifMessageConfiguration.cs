using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Messages
{
    public class GifMessageConfiguration : IEntityTypeConfiguration<GifMessage>
    {
        public void Configure(EntityTypeBuilder<GifMessage> builder)
        {
            builder.HasOne(e => e.File)
                   .WithOne(e => e.GifMessage)
                   .HasForeignKey<GifMessage>(e => e.FileId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
