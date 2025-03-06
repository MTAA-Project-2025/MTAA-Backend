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
    public class UserRelationshipConfiguration : IEntityTypeConfiguration<UserRelationship>
    {
        public void Configure(EntityTypeBuilder<UserRelationship> builder)
        {
            builder.HasKey(e => new { e.User1Id, e.User2Id });

            builder.HasOne(uc => uc.User1)
                .WithMany(u => u.UserRelationships1)
                .HasForeignKey(uc => uc.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(uc => uc.User2)
                .WithMany(e => e.UserRelationships2)
                .HasForeignKey(uc => uc.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.IsUser1Followig, e.IsUser2Followig });
        }
    }
}
