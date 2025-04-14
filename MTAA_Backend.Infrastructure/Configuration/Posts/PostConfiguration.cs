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
                   .HasForeignKey(e => e.PostId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Likes)
                   .WithOne(e => e.Post)
                   .HasForeignKey(e => e.PostId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Owner)
                   .WithMany(e => e.CreatedPosts)
                   .HasForeignKey(e => e.OwnerId);

            builder.HasMany(e => e.Comments)
                   .WithOne(e => e.Post)
                   .HasForeignKey(e => e.PostId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.WatchedUsers)
                   .WithMany(e => e.WatchedPosts);

            builder.HasMany(e => e.RecommendationItems)
                   .WithOne(e => e.Post)
                   .HasForeignKey(e => e.PostId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Notifications)
                    .WithOne(n => n.Post)
                    .HasForeignKey(n => n.PostId);

            builder.HasIndex(e => new { e.GlobalScore, e.CommentsCount, e.LikesCount, e.DataCreationTime, e.IsDeleted });
        }
    }
}
