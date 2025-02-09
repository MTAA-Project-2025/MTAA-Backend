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
    public class BaseGroupConfiguration : IEntityTypeConfiguration<BaseGroup>
    {
        public void Configure(EntityTypeBuilder<BaseGroup> builder)
        {
            builder.HasMany(e => e.Messages)
                   .WithOne(e => e.Group)
                   .HasForeignKey(e => e.GroupId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Participants)
                   .WithMany(e => e.Groups);
            
            builder.HasMany(e=>e.UserGroupMemberships)
                   .WithOne(e => e.Group)
                   .HasForeignKey(e => e.GroupId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.Type, e.Visibility });
        }
    }
}
