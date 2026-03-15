using Alti.Domain.Entities;
using Alti.Domain.Enums;

namespace Alti.Domain.Interfaces.Repositories;

public interface IRateRepository : IBaseRepository<Rate>
{
    Task<Rate?> GetBySeasonAndTypeAsync(int seasonId, RoomType roomType, CancellationToken ct = default);
    Task<IReadOnlyList<Rate>> GetBySeasonAsync(int seasonId, CancellationToken ct = default);
}