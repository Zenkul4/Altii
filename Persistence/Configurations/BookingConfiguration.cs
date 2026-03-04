using Alti.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("id")
            .UseIdentityColumn();

        builder.Property(b => b.Code)
            .HasColumnName("code")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(b => b.GuestId)
            .HasColumnName("guest_id")
            .IsRequired();

        builder.Property(b => b.RoomId)
            .HasColumnName("room_id")
            .IsRequired();

        builder.Property(b => b.AttendedById)
            .HasColumnName("attended_by_id");

        builder.Property(b => b.CheckInDate)
            .HasColumnName("check_in_date")
            .IsRequired();

        builder.Property(b => b.CheckOutDate)
            .HasColumnName("check_out_date")
            .IsRequired();

        builder.Property(b => b.PricePerNight)
            .HasColumnName("price_per_night")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(b => b.TotalPrice)
            .HasColumnName("total_price")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(b => b.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(b => b.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.Property(b => b.Notes)
            .HasColumnName("notes");

        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");

        builder.Property(b => b.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(b => b.Code)
            .IsUnique();

        builder.HasIndex(b => new { b.RoomId, b.CheckInDate, b.CheckOutDate });

        builder.HasIndex(b => b.GuestId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.GuestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Room>()
            .WithMany()
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.AttendedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}