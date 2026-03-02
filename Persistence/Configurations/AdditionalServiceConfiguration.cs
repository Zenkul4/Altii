using Alti.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AdditionalServiceConfiguration : IEntityTypeConfiguration<AdditionalService>
{
    public void Configure(EntityTypeBuilder<AdditionalService> builder)
    {
        builder.ToTable("additional_services");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id")
            .UseIdentityColumn();

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasColumnName("description");

        builder.Property(s => s.Price)
            .HasColumnName("price")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(s => s.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(s => s.Name)
            .IsUnique();

        builder.HasMany(s => s.BookingServices)
            .WithOne(bs => bs.Service)
            .HasForeignKey(bs => bs.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}