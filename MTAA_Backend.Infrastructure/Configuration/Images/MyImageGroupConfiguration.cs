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
    public class MyImageGroupConfiguration : IEntityTypeConfiguration<MyImageGroup>
    {
        public void Configure(EntityTypeBuilder<MyImageGroup> builder)
        {
            builder.HasOne(e => e.UserAvatar)
                   .WithOne(e => e.CustomAvatar)
                   .HasForeignKey<MyImageGroup>(e => e.UserAvatarId)
                   .IsRequired(false);

            builder.HasMany(e => e.Images)
                   .WithOne(e => e.ImageGroup)
                   .HasForeignKey(e => e.ImageGroupId);
        }
    }
}
