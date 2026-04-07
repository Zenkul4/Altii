using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class BookingRepository : BaseRepository<Booking>, IBookingRepository, IBookingAdminRepository
{
    public BookingRepository(AppDbContext context) : base(context) { }
    public async Task<IReadOnlyList<Booking>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
        => await DbSet
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<Booking?> GetByCodeAsync(string code, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(b => b.Code == code.ToUpper(), ct);

    public async Task<IReadOnlyList<Booking>> GetByGuestAsync(
        int guestId, int page, int pageSize, CancellationToken ct = default)
        => await DbSet
            .Where(b => b.GuestId == guestId)
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Booking>> GetActiveAsync(
        int page, int pageSize, CancellationToken ct = default)
        => await DbSet
            .Where(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.CheckedIn)
            .OrderBy(b => b.CheckInDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Booking>> GetExpiredPendingAsync(CancellationToken ct = default)
        => await DbSet
            .Where(b => b.Status == BookingStatus.PendingPayment && b.ExpiresAt <= DateTimeOffset.UtcNow)
            .ToListAsync(ct);

    public async Task<bool> HasConflictAsync(
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int? excludeBookingId = null,
        CancellationToken ct = default)
    {
        var query = DbSet.Where(b =>
            b.RoomId == roomId &&
            b.Status != BookingStatus.Cancelled &&
            b.Status != BookingStatus.Expired &&
            b.Status != BookingStatus.CheckedOut &&
            b.CheckInDate < checkOutDate &&
            b.CheckOutDate > checkInDate);

        if (excludeBookingId.HasValue)
            query = query.Where(b => b.Id != excludeBookingId.Value);

        return await query.AnyAsync(ct);
    }
}