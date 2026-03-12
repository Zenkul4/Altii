using Alti.Domain.Entities;
using Alti.Domain.Enums;

namespace Alti.Domain.Interfaces.Repositories;

public interface IRoomRepository : IBaseRepository<Room>
{
    Task<Room?> GetByNumberAsync(string number, CancellationToken ct = default);
    Task<IReadOnlyList<Room>> GetAvailableAsync(
        DateOnly checkInDate,
        DateOnly checkOutDate,
        RoomType? type = null,
        short? minCapacity = null,
        CancellationToken ct = default);
}