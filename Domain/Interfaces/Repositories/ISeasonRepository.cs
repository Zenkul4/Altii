using Alti.Domain.Entities;

namespace Alti.Domain.Interfaces.Repositories;

public interface ISeasonRepository : IBaseRepository<Season>
{
    Task<Season?> GetActiveForDateAsync(DateOnly date, CancellationToken ct = default);
    Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken ct = default);
    Task<bool> HasOverlapAsync(
        DateOnly startDate,
        DateOnly endDate,
        int? excludeSeasonId = null,
        CancellationToken ct = default);
}