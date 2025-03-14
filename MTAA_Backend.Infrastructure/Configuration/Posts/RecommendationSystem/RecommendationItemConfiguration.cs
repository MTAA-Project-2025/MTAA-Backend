using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Posts.RecommendationSystem
{
    public class RecommendationItemConfiguration : IEntityTypeConfiguration<RecommendationItem>
    {
        public void Configure(EntityTypeBuilder<RecommendationItem> builder)
        {
            builder.HasOne(e => e.Post)
                   .WithMany(e => e.RecommendationItems)
                   .HasForeignKey(e => e.PostId);

            builder.HasOne(e => e.Feed)
                   .WithMany(e => e.RecommendationItems)
                   .HasForeignKey(e => e.FeedId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(e => e.LocalScore);
        }
    }
}
