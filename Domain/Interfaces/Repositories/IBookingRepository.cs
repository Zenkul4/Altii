using Alti.Domain.Entities;

namespace Alti.Domain.Interfaces.Repositories;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Booking?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetByGuestAsync(int guestId, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetActiveAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetExpiredPendingAsync(CancellationToken ct = default);
    Task<bool> HasConflictAsync(int roomId, DateOnly checkInDate, DateOnly checkOutDate, int? excludeBookingId = null, CancellationToken ct = default);
    Task AddAsync(Booking booking, CancellationToken ct = default);
    void Update(Booking booking);
}