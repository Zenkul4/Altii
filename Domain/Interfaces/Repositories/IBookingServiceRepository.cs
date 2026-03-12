using Alti.Domain.Entities;

namespace Alti.Domain.Interfaces.Repositories;

public interface IBookingServiceRepository : IBaseRepository<BookingService>
{
    Task<IReadOnlyList<BookingService>> GetByBookingAsync(int bookingId, CancellationToken ct = default);
}