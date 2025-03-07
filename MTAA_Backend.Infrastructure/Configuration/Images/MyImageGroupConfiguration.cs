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

            builder.HasOne(e => e.Channel)
                   .WithOne(e => e.Image)
                   .HasForeignKey<MyImageGroup>(e => e.ChannelId)
                   .IsRequired(false);

            builder.HasOne(e => e.Message)
                   .WithMany(e => e.Images)
                   .HasForeignKey(e => e.MessageId)
                   .IsRequired(false);

            builder.HasOne(e => e.Post)
                   .WithMany(e => e.Images)
                   .HasForeignKey(e => e.PostId)
                   .IsRequired(false);
        }
    }
}
