using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Resources.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Seeders.Images
{
    internal class MyImageSeeder : IEntityTypeConfiguration<MyImage>
    {
        public void Configure(EntityTypeBuilder<MyImage> builder)
        {
            builder.HasData(
                new MyImage()
                {
                    Id = Guid.Parse("bf309314-7efa-4b60-a021-0b17a7a5da6f"),
                    ImageGroupId = Guid.Parse("416c7d33-0a25-4176-b783-64b25919ac12"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_1_100.jpg",
                    AspectRatio = 1.0,
                    Width = 100,
                    Height = 100,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_1_100"
                },
                new MyImage()
                {
                    Id = Guid.Parse("77f98ba4-1961-435c-93c6-c351572e5837"),
                    ImageGroupId = Guid.Parse("416c7d33-0a25-4176-b783-64b25919ac12"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_1_300.jpg",
                    AspectRatio = 1.0,
                    Width = 300,
                    Height = 300,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_1_300"
                },
                new MyImage()
                {
                    Id = Guid.Parse("5f3d5283-4fd2-4194-a4d6-345f83c967b3"),
                    ImageGroupId = Guid.Parse("161750a4-9b50-4a1c-a5f1-3221640533c6"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_2_100.jpg",
                    AspectRatio = 1.0,
                    Width = 100,
                    Height = 100,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_2_100"
                },
                new MyImage()
                {
                    Id = Guid.Parse("ed659976-0f06-4d6a-ad9e-9456e4a82d3c"),
                    ImageGroupId = Guid.Parse("161750a4-9b50-4a1c-a5f1-3221640533c6"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_2_300.jpg",
                    AspectRatio = 1.0,
                    Width = 300,
                    Height = 300,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_2_300"
                },
                new MyImage()
                {
                    Id = Guid.Parse("580c86e5-f708-44d2-aba1-d00b6d311ce1"),
                    ImageGroupId = Guid.Parse("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_3_100.jpg",
                    AspectRatio = 1.0,
                    Width = 100,
                    Height = 100,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_3_100"
                },
                new MyImage()
                {
                    Id = Guid.Parse("331f73e4-2035-45fe-9e0d-33a8a930b922"),
                    ImageGroupId = Guid.Parse("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_3_300.jpg",
                    AspectRatio = 1.0,
                    Width = 300,
                    Height = 300,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_3_300"
                },
                new MyImage()
                {
                    Id = Guid.Parse("4c64dbaf-3ecf-468e-8265-bc9233fc2c7e"),
                    ImageGroupId = Guid.Parse("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_4_100.jpg",
                    AspectRatio = 1.0,
                    Width = 100,
                    Height = 100,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_4_100"
                },
                new MyImage()
                {
                    Id = Guid.Parse("3efe30aa-4cfa-4003-878d-054de78ea07b"),
                    ImageGroupId = Guid.Parse("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_4_300.jpg",
                    AspectRatio = 1.0,
                    Width = 300,
                    Height = 300,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_4_300"
                },
                new MyImage()
                {
                    Id = Guid.Parse("3bead263-b9fd-4a6f-a649-c699969863b2"),
                    ImageGroupId = Guid.Parse("9ad61bee-053b-4042-8b4a-860fe80dd05a"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_5_100.jpg",
                    AspectRatio = 1.0,
                    Width = 100,
                    Height = 100,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_5_100"
                },
                new MyImage()
                {
                    Id = Guid.Parse("2ea5ce43-807b-4419-a506-d68c7cba07a4"),
                    ImageGroupId = Guid.Parse("9ad61bee-053b-4042-8b4a-860fe80dd05a"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_5_300.jpg",
                    AspectRatio = 1.0,
                    Width = 300,
                    Height = 300,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_5_300"
                },
                new MyImage()
                {
                    Id = Guid.Parse("d0c6bab5-1b86-4e29-8bcd-34e5ca1d9f2b"),
                    ImageGroupId = Guid.Parse("d1a56d08-a7de-4855-8a13-5fbda2ca4843"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_6_100.jpg",
                    AspectRatio = 1.0,
                    Width = 100,
                    Height = 100,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_6_100"
                },
                new MyImage()
                {
                    Id = Guid.Parse("f9ab80f0-cc7f-4f3a-83fe-e25c9e81253b"),
                    ImageGroupId = Guid.Parse("d1a56d08-a7de-4855-8a13-5fbda2ca4843"),
                    FullPath = "https://mtaafiles.blob.core.windows.net/images/userAvatar_6_300.jpg",
                    AspectRatio = 1.0,
                    Width = 300,
                    Height = 300,
                    FileType = ImagesFileTypes.Jpg,
                    ShortPath = "userAvatar_6_300"
                });
        }
    }
}
