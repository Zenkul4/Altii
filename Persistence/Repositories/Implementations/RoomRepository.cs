using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class RoomRepository : BaseRepository<Room>, IRoomRepository, IRoomAdminRepository

{
    public RoomRepository(AppDbContext context) : base(context) { }

    public async Task<Room?> GetByNumberAsync(string number, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(r => r.Number == number.ToUpper(), ct);

    public async Task<IReadOnlyList<Room>> GetAvailableAsync(
        DateOnly checkInDate,
        DateOnly checkOutDate,
        RoomType? type = null,
        short? minCapacity = null,
        CancellationToken ct = default)
    {
        var occupiedRoomIds = await Context.Bookings
            .Where(b =>
                b.Status != BookingStatus.Cancelled &&
                b.Status != BookingStatus.Expired &&
                b.Status != BookingStatus.CheckedOut &&
                b.CheckInDate < checkOutDate &&
                b.CheckOutDate > checkInDate)
            .Select(b => b.RoomId)
            .Distinct()
            .ToListAsync(ct);

        var query = DbSet
            .Where(r => r.Status == RoomStatus.Available && !occupiedRoomIds.Contains(r.Id));

        if (type.HasValue)
            query = query.Where(r => r.Type == type.Value);

        if (minCapacity.HasValue)
            query = query.Where(r => r.Capacity >= minCapacity.Value);

        return await query.ToListAsync(ct);
    }
    public async Task<IReadOnlyList<Room>> GetAllAsync(CancellationToken ct = default)
    => await DbSet.ToListAsync(ct);

    async Task<Room?> IRoomAdminRepository.GetByIdAsync(int id, CancellationToken ct)
        => await DbSet.FirstOrDefaultAsync(r => r.Id == id, ct);
}