using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTAA_Backend.Domain.Entities.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Metadata;

namespace MTAA_Backend.Infrastructure.Configuration.Locations
{
    public class LocationPointConfiguration : IEntityTypeConfiguration<LocationPoint>
    {
        public void Configure(EntityTypeBuilder<LocationPoint> builder)
        {
            builder.HasOne(e => e.Location)
                .WithMany(e => e.Points)
                .HasForeignKey(e => e.LocationId)
                .IsRequired(false);

            builder.HasOne(e => e.Parent)
                .WithMany(e => e.LocationPoints)
                .HasForeignKey(e => e.ParentId);

            builder.HasMany(e => e.LocationPoints)
                .WithOne(e => e.Parent)
                .HasForeignKey(e => e.ParentId);

            builder.Property(e => e.Coordinates)
                   .HasColumnType("geography (point)");

            builder.HasIndex(e => new { e.Type, e.ZoomLevel, e.IsSubPoint, e.IsVisible });
        }
    }
}
