using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Images
{
    public class MyImageConfiguration : IEntityTypeConfiguration<MyImage>
    {
        public void Configure(EntityTypeBuilder<MyImage> builder)
        {
            builder.HasOne(e => e.ImageGroup)
                   .WithMany(e => e.Images)
                   .HasForeignKey(e => e.ImageGroupId);

            builder.HasIndex(e => e.Type);
        }
    }
}
