using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Posts.RecomendationSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Posts.RecomendationSystem
{
    public class RecomendationFeedConfiguration : IEntityTypeConfiguration<RecomendationFeed>
    {
        public void Configure(EntityTypeBuilder<RecomendationFeed> builder)
        {
            builder.HasMany(e => e.RecomendationItems)
                   .WithOne(e => e.Feed)
                   .HasForeignKey(e => e.FeedId);

            builder.HasOne(e => e.User)
                   .WithMany(e => e.RecomendationFeeds)
                   .HasForeignKey(e => e.UserId);


            builder.HasIndex(e => new { e.Type, e.Weight });
        }
    }
}
