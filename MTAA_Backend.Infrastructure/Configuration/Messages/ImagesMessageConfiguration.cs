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
    public class ImagesMessageConfiguration : IEntityTypeConfiguration<ImagesMessage>
    {
        public void Configure(EntityTypeBuilder<ImagesMessage> builder)
        {
            builder.HasMany(e => e.Images)
                   .WithOne(e => e.Message)
                   .HasForeignKey(e => e.MessageId);
        }
    }
}
