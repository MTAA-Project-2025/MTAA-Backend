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
    public class SharedRecommendationFeedConfiguration : IEntityTypeConfiguration<SharedRecommendationFeed>
    {
        public void Configure(EntityTypeBuilder<SharedRecommendationFeed> builder)
        {
            builder.HasMany(e => e.Users)
                   .WithMany(e => e.SharedRecommendationFeeds);
        }
    }
}
