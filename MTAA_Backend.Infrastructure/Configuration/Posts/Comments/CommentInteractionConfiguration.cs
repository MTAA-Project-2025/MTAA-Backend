using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.Entities.Posts.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Posts.Comments
{
    public class CommentInteractionConfiguration : IEntityTypeConfiguration<CommentInteraction>
    {
        public void Configure(EntityTypeBuilder<CommentInteraction> builder)
        {
            builder.HasKey(ci => new { ci.UserId, ci.CommentId });

            builder.HasOne(ci => ci.User)
                .WithMany(e => e.CommentInteractions)
                .HasForeignKey(ci => ci.UserId);

            builder.HasOne(ci => ci.Comment)
                .WithMany(e => e.CommentInteractions)
                .HasForeignKey(ci => ci.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ci => ci.Type);
        }
    }

}
