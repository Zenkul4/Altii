using Alti.Domain.Entities;
using Alti.Domain.Enums;

namespace Alti.Domain.Interfaces.Repositories;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Room?> GetByNumberAsync(string number, CancellationToken ct = default);
    Task<IReadOnlyList<Room>> GetAvailableAsync(
        DateOnly checkInDate,
        DateOnly checkOutDate,
        RoomType? type = null,
        short? minCapacity = null,
        CancellationToken ct = default);
    Task AddAsync(Room room, CancellationToken ct = default);
    void Update(Room room);
}
