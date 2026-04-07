using Alti.Domain.Entities;
using Alti.Domain.Interfaces;

namespace Alti.Domain.Interfaces.Repositories;

public interface IRoomAdminRepository
{
    Task<IReadOnlyList<Room>> GetAllAsync(CancellationToken ct = default);
    Task<Room?> GetByIdAsync(int id, CancellationToken ct = default);
}