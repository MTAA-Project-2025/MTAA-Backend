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
    public class BaseRecommendationFeedConfiguration : IEntityTypeConfiguration<BaseRecommendationFeed>
    {
        public void Configure(EntityTypeBuilder<BaseRecommendationFeed> builder)
        {
            builder.HasMany(e => e.RecommendationItems)
                   .WithOne(e => e.Feed)
                   .HasForeignKey(e => e.FeedId);

            builder.HasIndex(e => new { e.Type, e.RecommendationItemsCount });
        }
    }
}
