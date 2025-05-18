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
    public class FirebaseItemConfiguration : IEntityTypeConfiguration<FirebaseItem>
    {
        public void Configure(EntityTypeBuilder<FirebaseItem> builder)
        {
            builder.HasOne(e => e.User)
                   .WithMany(e => e.FirebaseItems)
                   .HasForeignKey(e => e.UserId);
        }
    }
}
