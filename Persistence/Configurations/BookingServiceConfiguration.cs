using Alti.Domain.Entities;
using Core.domain.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class BookingServiceConfiguration : IEntityTypeConfiguration<BookingService>
{
    public void Configure(EntityTypeBuilder<BookingService> builder)
    {
        builder.ToTable("booking_services");

        builder.HasKey(bs => bs.Id);

        builder.Property(bs => bs.Id)
            .HasColumnName("id")
            .UseIdentityColumn();

        builder.Property(bs => bs.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();

        builder.Property(bs => bs.ServiceId)
            .HasColumnName("service_id")
            .IsRequired();

        builder.Property(bs => bs.RegisteredById)
            .HasColumnName("registered_by_id")
            .IsRequired();

        builder.Property(bs => bs.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(bs => bs.UnitPrice)
            .HasColumnName("unit_price")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(bs => bs.RegisteredAt)
            .HasColumnName("registered_at")
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(bs => bs.BookingId);

        builder.HasOne(bs => bs.RegisteredBy)
            .WithMany()
            .HasForeignKey(bs => bs.RegisteredById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
