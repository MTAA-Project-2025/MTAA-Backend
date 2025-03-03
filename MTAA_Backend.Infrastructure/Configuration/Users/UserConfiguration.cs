using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(e => e.Groups)
                   .WithMany(e => e.Participants);

            builder.HasMany(e => e.Messages)
                   .WithOne(e => e.Sender)
                   .HasForeignKey(e => e.SenderId);

            builder.HasOne(e => e.Avatar)
                   .WithOne(e => e.User)
                   .HasForeignKey<User>(e => e.AvatarId)
                   .IsRequired(false);

            builder.HasMany(e => e.Contacts)
                   .WithOne(e => e.User)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ContactOf)
                   .WithOne(e => e.Contact)
                   .HasForeignKey(e => e.ContactId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.UserGroupMemberships)
                   .WithOne(e => e.User)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.OwnedChannels)
                   .WithOne(e => e.Owner)
                   .HasForeignKey(e => e.OwnerId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(e => e.LikedPosts)
                   .WithMany(e => e.LikedUsers);

            builder.HasMany(e => e.CreatedPosts)
                   .WithOne(e => e.Owner)
                   .HasForeignKey(e => e.OwnerId);
        }
    }
}
