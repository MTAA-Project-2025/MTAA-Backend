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
    public class LocalRecommendationFeedConfiguration : IEntityTypeConfiguration<LocalRecommendationFeed>
    {
        public void Configure(EntityTypeBuilder<LocalRecommendationFeed> builder)
        {
            builder.HasOne(e => e.User)
                   .WithMany(e => e.LocalRecommendationFeeds)
                   .HasForeignKey(e => e.UserId);

            builder.HasIndex(e => new { e.IsActive });
        }
    }
}
