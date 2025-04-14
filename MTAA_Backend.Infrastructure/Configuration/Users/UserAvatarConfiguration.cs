using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Users
{
    internal class UserAvatarConfiguration : IEntityTypeConfiguration<UserAvatar>
    {
        public void Configure(EntityTypeBuilder<UserAvatar> builder)
        {
            builder.HasOne(e=>e.User)
                .WithOne(e => e.Avatar)
                .HasForeignKey<UserAvatar>(e => e.UserId)
                .IsRequired(false);

            builder.HasOne(e => e.CustomAvatar)
                .WithOne(e => e.UserAvatar)
                .HasForeignKey<UserAvatar>(e => e.CustomAvatarId)
                .IsRequired(false);

            builder.HasOne(e => e.PresetAvatar)
                .WithMany(e => e.UserAvatars)
                .HasForeignKey(e => e.PresetAvatarId)
                .IsRequired(false);
        }
    }
}
