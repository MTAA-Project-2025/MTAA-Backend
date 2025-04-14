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
    public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
    {
        public void Configure(EntityTypeBuilder<PostLike> builder)
        {
            builder.HasOne(e => e.User)
                .WithMany(e => e.LikedPosts)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Post)
                   .WithMany(e => e.Likes)
                   .HasForeignKey(e => e.PostId);

            builder.HasKey(e => new { e.DataCreationTime });
        }
    }
}
