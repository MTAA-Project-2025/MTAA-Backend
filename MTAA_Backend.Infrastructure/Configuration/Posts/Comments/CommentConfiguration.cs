using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Posts.Comments;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Posts.Comments
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(e => e.Post)
                   .WithMany(e => e.Comments)
                   .HasForeignKey(e => e.PostId);

            builder.HasMany(e => e.ChildComments)
                   .WithOne(e => e.ParentComment)
                   .HasForeignKey(e => e.ParentCommentId);

            builder.HasKey(e => new { e.DataCreationTime });
        }
    }
}
