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
    public class UserGroupMembershipConfiguration : IEntityTypeConfiguration<UserGroupMembership>
    {
        public void Configure(EntityTypeBuilder<UserGroupMembership> builder)
        {
            builder.HasOne(e => e.User)
                   .WithMany(e => e.UserGroupMemberships)
                   .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Group)
                     .WithMany(e => e.UserGroupMemberships)
                     .HasForeignKey(e => e.GroupId);

            builder.HasOne(e => e.LastMessage)
                     .WithMany(e => e.LastMessageUserGroupMemberships)
                     .HasForeignKey(e => e.LastMessageId)
                     .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(e => new { e.IsNotificationEnabled, e.IsArchived, e.UnreadMessagesCount });
        }
    }
}
