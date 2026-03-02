using Alti.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class RateConfiguration : IEntityTypeConfiguration<Rate>
{
    public void Configure(EntityTypeBuilder<Rate> builder)
    {
        builder.ToTable("rates");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .UseIdentityColumn();

        builder.Property(r => r.SeasonId)
            .HasColumnName("season_id")
            .IsRequired();

        builder.Property(r => r.RoomType)
            .HasColumnName("room_type")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(r => r.PricePerNight)
            .HasColumnName("price_per_night")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(r => r.CreatedById)
            .HasColumnName("created_by_id")
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");

        builder.Property(r => r.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(r => new { r.SeasonId, r.RoomType })
            .IsUnique();

        builder.HasOne(r => r.CreatedBy)
            .WithMany()
            .HasForeignKey(r => r.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}