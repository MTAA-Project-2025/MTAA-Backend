using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Entities.Versions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Versions
{
    public class VersionItemConfiguration : IEntityTypeConfiguration<VersionItem>
    {
        public void Configure(EntityTypeBuilder<VersionItem> builder)
        {
            builder.HasKey(v => new { v.Id, v.UserId });

            builder.HasOne(v => v.User)
                .WithMany(u => u.VersionItems)
                .HasForeignKey(v => v.UserId);
        }
    }
}
