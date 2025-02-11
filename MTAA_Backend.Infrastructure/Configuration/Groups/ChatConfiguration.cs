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
    internal class ChatConfiguration : IEntityTypeConfiguration<ContactChat>
    {
        public void Configure(EntityTypeBuilder<ContactChat> builder)
        {
            builder.HasOne(e => e.User)
                   .WithMany(e => e.OwnedChats)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.NoAction);


            builder.HasIndex(e => new { e.IdentificationName });
        }
    }
}
