using Alti.Domain.Entities;
using Alti.Domain.Enums;

namespace Alti.Domain.Interfaces.Repositories;

public interface IRateRepository
{
    Task<Rate?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Rate?> GetBySeasonAndTypeAsync(int seasonId, RoomType roomType, CancellationToken ct = default);
    Task<IReadOnlyList<Rate>> GetBySeasonAsync(int seasonId, CancellationToken ct = default);
    Task AddAsync(Rate rate, CancellationToken ct = default);
    void Update(Rate rate);
}