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
    public class UserPresetAvatarImageConfiguration : IEntityTypeConfiguration<UserPresetAvatarImage>
    {
        public void Configure(EntityTypeBuilder<UserPresetAvatarImage> builder)
        {
            builder.HasMany(e => e.UserAvatars)
                   .WithOne(e => e.PresetAvatar)
                   .HasForeignKey(e => e.PresetAvatarId);

        }
    }
}
