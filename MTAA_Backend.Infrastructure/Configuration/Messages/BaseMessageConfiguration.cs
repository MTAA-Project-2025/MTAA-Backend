using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Messages
{
    public class BaseMessageConfiguration : IEntityTypeConfiguration<BaseMessage>
    {
        public void Configure(EntityTypeBuilder<BaseMessage> builder)
        {
            builder.HasOne(e => e.Group)
                   .WithMany(e => e.Messages)
                   .HasForeignKey(e => e.GroupId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Sender)
                   .WithMany(e => e.Messages)
                   .HasForeignKey(e => e.SenderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.LastMessageUserGroupMemberships)
                   .WithOne(e => e.LastMessage)
                   .HasForeignKey(e => e.LastMessageId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
