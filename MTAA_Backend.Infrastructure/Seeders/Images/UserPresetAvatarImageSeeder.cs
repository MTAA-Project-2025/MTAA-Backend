using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Seeders.Images
{
    internal class UserPresetAvatarImageSeeder : IEntityTypeConfiguration<UserPresetAvatarImage>
    {
        public void Configure(EntityTypeBuilder<UserPresetAvatarImage> builder)
        {
            builder.HasData(
                new UserPresetAvatarImage()
                {
                    Id = Guid.Parse("416c7d33-0a25-4176-b783-64b25919ac12"),
                    Title = "Preset Avatar",
                    DataCreationTime=DateTime.MinValue,
                },
                new UserPresetAvatarImage()
                {
                    Id = Guid.Parse("161750a4-9b50-4a1c-a5f1-3221640533c6"),
                    Title = "Preset Avatar",
                    DataCreationTime = DateTime.MinValue,
                },
                new UserPresetAvatarImage()
                {
                    Id = Guid.Parse("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"),
                    Title = "Preset Avatar",
                    DataCreationTime = DateTime.MinValue,
                },
                new UserPresetAvatarImage()
                {
                    Id = Guid.Parse("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"),
                    Title = "Preset Avatar",
                    DataCreationTime = DateTime.MinValue,
                },
                new UserPresetAvatarImage()
                {
                    Id = Guid.Parse("9ad61bee-053b-4042-8b4a-860fe80dd05a"),
                    Title = "Preset Avatar",
                    DataCreationTime = DateTime.MinValue,
                },
                new UserPresetAvatarImage()
                {
                    Id = Guid.Parse("d1a56d08-a7de-4855-8a13-5fbda2ca4843"),
                    Title = "Preset Avatar",
                    DataCreationTime = DateTime.MinValue,
                });
        }
    }
}
