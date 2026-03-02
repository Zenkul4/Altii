using Alti.Domain.Entities;
using Alti.Domain.Factories.Interfaces;

namespace Alti.Domain.Factories.Implementations;

public class BookingServiceFactory : IBookingServiceFactory
{
    public BookingService Create(
        int bookingId,
        int serviceId,
        int registeredById,
        short quantity,
        decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));

        return new BookingService
        {
            BookingId = bookingId,
            ServiceId = serviceId,
            RegisteredById = registeredById,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
}