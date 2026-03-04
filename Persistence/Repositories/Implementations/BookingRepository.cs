using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<Booking?> GetByCodeAsync(string code, CancellationToken ct = default)
        => await _context.Bookings.FirstOrDefaultAsync(b => b.Code == code.ToUpper(), ct);

    public async Task<IReadOnlyList<Booking>> GetByGuestAsync(int guestId, CancellationToken ct = default)
        => await _context.Bookings
            .Where(b => b.GuestId == guestId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Booking>> GetActiveAsync(CancellationToken ct = default)
        => await _context.Bookings
            .Where(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.CheckedIn)
            .OrderBy(b => b.CheckInDate)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Booking>> GetExpiredPendingAsync(CancellationToken ct = default)
        => await _context.Bookings
            .Where(b => b.Status == BookingStatus.PendingPayment && b.ExpiresAt <= DateTimeOffset.UtcNow)
            .ToListAsync(ct);

    public async Task<bool> HasConflictAsync(
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int? excludeBookingId = null,
        CancellationToken ct = default)
    {
        var query = _context.Bookings.Where(b =>
            b.RoomId == roomId &&
            b.Status != BookingStatus.Cancelled &&
            b.Status != BookingStatus.Expired &&
            b.CheckInDate < checkOutDate &&
            b.CheckOutDate > checkInDate);

        if (excludeBookingId.HasValue)
            query = query.Where(b => b.Id != excludeBookingId.Value);

        return await query.AnyAsync(ct);
    }

    public async Task AddAsync(Booking booking, CancellationToken ct = default)
        => await _context.Bookings.AddAsync(booking, ct);

    public void Update(Booking booking)
        => _context.Bookings.Update(booking);
}