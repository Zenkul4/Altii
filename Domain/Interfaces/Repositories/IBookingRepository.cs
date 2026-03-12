using Alti.Domain.Entities;

namespace Alti.Domain.Interfaces.Repositories;

public interface IBookingRepository : IBaseRepository<Booking>
{
    Task<Booking?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetByGuestAsync(int guestId, int page, int pageSize, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetActiveAsync(int page, int pageSize, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetExpiredPendingAsync(CancellationToken ct = default);
    Task<bool> HasConflictAsync(
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int? excludeBookingId = null,
        CancellationToken ct = default);
}