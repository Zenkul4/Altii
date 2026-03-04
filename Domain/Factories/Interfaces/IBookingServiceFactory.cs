using Alti.Domain.Entities;

namespace Alti.Domain.Factories.Interfaces;

public interface IBookingServiceFactory
{
    BookingService Create(
        int bookingId,
        int serviceId,
        int registeredById,
        short quantity,
        decimal unitPrice);
}