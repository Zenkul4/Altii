using Alti.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("rooms");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .UseIdentityColumn();

        builder.Property(r => r.Number)
            .HasColumnName("number")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(r => r.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(r => r.Floor)
            .HasColumnName("floor")
            .IsRequired();

        builder.Property(r => r.Capacity)
            .HasColumnName("capacity")
            .IsRequired();

        builder.Property(r => r.BasePrice)
            .HasColumnName("base_price")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(r => r.Description)
            .HasColumnName("description");

        builder.Property(r => r.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(r => r.RowVersion)
            .HasColumnName("row_version")
            .IsConcurrencyToken();

        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");

        builder.Property(r => r.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(r => r.Number)
            .IsUnique();

    }
}