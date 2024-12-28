using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Chats;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Chats
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasMany(e => e.Messages)
                   .WithOne(e => e.Chat)
                   .HasForeignKey(e => e.ChatId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Participants)
                   .WithMany(e => e.Chats);
        }
    }
}
