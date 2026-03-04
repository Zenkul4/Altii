using Alti.Domain.Entities;
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

        builder.HasOne<Booking>()
            .WithMany()
            .HasForeignKey(bs => bs.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<AdditionalService>()
            .WithMany()
            .HasForeignKey(bs => bs.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(bs => bs.RegisteredById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}