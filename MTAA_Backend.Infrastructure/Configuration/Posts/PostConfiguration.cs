using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Posts
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasMany(e => e.Images)
                   .WithOne(e => e.Post)
                   .HasForeignKey(e => e.PostId);

            builder.HasMany(e => e.LikedUsers)
                   .WithMany(e => e.LikedPosts);

            builder.HasOne(e => e.Owner)
                   .WithMany(e => e.CreatedPosts)
                   .HasForeignKey(e => e.OwnerId);

            builder.HasOne(e => e.Location)
                   .WithOne(e => e.Post)
                   .HasForeignKey<Post>(e => e.LocationId)
                   .IsRequired(false);

            builder.HasMany(e => e.Comments)
                   .WithOne(e => e.Post)
                   .HasForeignKey(e => e.PostId);
        }
    }
}
