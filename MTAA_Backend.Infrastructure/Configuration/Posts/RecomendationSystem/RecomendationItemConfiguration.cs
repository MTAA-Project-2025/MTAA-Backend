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
    public class RecomendationItemConfiguration : IEntityTypeConfiguration<RecomendationItem>
    {
        public void Configure(EntityTypeBuilder<RecomendationItem> builder)
        {
            builder.HasOne(e => e.Post)
                   .WithMany(e => e.RecomendationItems)
                   .HasForeignKey(e => e.PostId);

            builder.HasOne(e => e.Feed)
                   .WithMany(e => e.RecomendationItems)
                   .HasForeignKey(e => e.FeedId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(e => e.LocalScore);

        }
    }
}
