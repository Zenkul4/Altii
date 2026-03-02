using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Factories.Interfaces;

namespace Alti.Domain.Factories.Implementations;

public class BookingFactory : IBookingFactory
{
    public Booking Create(
        string code,
        int guestId,
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        decimal pricePerNight,
        int? attendedById = null,
        string? notes = null,
        int expirationMinutes = 15)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Booking code cannot be empty.", nameof(code));

        if (checkOutDate <= checkInDate)
            throw new ArgumentException("Check-out date must be after check-in date.", nameof(checkOutDate));

        if (pricePerNight <= 0)
            throw new ArgumentException("Price per night must be greater than zero.", nameof(pricePerNight));

        if (expirationMinutes <= 0)
            throw new ArgumentException("Expiration minutes must be greater than zero.", nameof(expirationMinutes));

        var nights = checkOutDate.DayNumber - checkInDate.DayNumber;
        var totalPrice = Math.Round(pricePerNight * nights, 2);

        return new Booking
        {
            Code = code.Trim().ToUpper(),
            GuestId = guestId,
            RoomId = roomId,
            AttendedById = attendedById,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate,
            PricePerNight = pricePerNight,
            TotalPrice = totalPrice,
            Notes = notes?.Trim(),
            Status = BookingStatus.PendingPayment,
            ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(expirationMinutes)
        };
    }
}