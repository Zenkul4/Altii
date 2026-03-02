using Alti.Domain.Entities;

namespace Alti.Domain.Factories.Interfaces;

public interface IBookingFactory
{
    Booking Create(
        string code,
        int guestId,
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        decimal pricePerNight,
        int? attendedById = null,
        string? notes = null,
        int expirationMinutes = 15);
}