using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Groups
{
    internal class ChannelConfiguration : IEntityTypeConfiguration<Channel>
    {
        public void Configure(EntityTypeBuilder<Channel> builder)
        {
            builder.HasOne(e => e.Image)
                   .WithOne(e => e.Channel)
                   .HasForeignKey<Channel>(e => e.ImageId)
                   .IsRequired(false);

            builder.HasOne(e => e.Owner)
                   .WithMany(e => e.OwnedChannels)
                   .HasForeignKey(e => e.OwnerId)
                   .OnDelete(DeleteBehavior.NoAction);


            builder.HasIndex(e => new { e.IdentificationName, e.DisplayName });
        }
    }
}
