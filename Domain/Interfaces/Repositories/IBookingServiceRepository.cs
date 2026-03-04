using Alti.Domain.Entities;

namespace Alti.Domain.Interfaces.Repositories;

public interface IBookingServiceRepository
{
    Task<BookingService?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<BookingService>> GetByBookingAsync(int bookingId, CancellationToken ct = default);
    Task AddAsync(BookingService bookingService, CancellationToken ct = default);
    void Update(BookingService bookingService);
}