using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Configuration.Messages
{
    public class VoiceMessageConfiguration : IEntityTypeConfiguration<VoiceMessage>
    {
        public void Configure(EntityTypeBuilder<VoiceMessage> builder)
        {
            builder.HasOne(e => e.File)
                   .WithOne(e => e.VoiceMessage)
                   .HasForeignKey<VoiceMessage>(e => e.FileId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
