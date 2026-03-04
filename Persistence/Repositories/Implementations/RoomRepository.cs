using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Room?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<Room?> GetByNumberAsync(string number, CancellationToken ct = default)
        => await _context.Rooms.FirstOrDefaultAsync(r => r.Number == number.ToUpper(), ct);

    public async Task<IReadOnlyList<Room>> GetAvailableAsync(
        DateOnly checkInDate,
        DateOnly checkOutDate,
        RoomType? type = null,
        short? minCapacity = null,
        CancellationToken ct = default)
    {
        var occupiedRoomIds = await _context.Bookings
            .Where(b =>
                b.Status != BookingStatus.Cancelled &&
                b.Status != BookingStatus.Expired &&
                b.CheckInDate < checkOutDate &&
                b.CheckOutDate > checkInDate)
            .Select(b => b.RoomId)
            .Distinct()
            .ToListAsync(ct);

        var query = _context.Rooms
            .Where(r => r.Status == RoomStatus.Available && !occupiedRoomIds.Contains(r.Id));

        if (type.HasValue)
            query = query.Where(r => r.Type == type.Value);

        if (minCapacity.HasValue)
            query = query.Where(r => r.Capacity >= minCapacity.Value);

        return await query.ToListAsync(ct);
    }

    public async Task AddAsync(Room room, CancellationToken ct = default)
        => await _context.Rooms.AddAsync(room, ct);

    public void Update(Room room)
        => _context.Rooms.Update(room);
}