using Alti.Domain.Entities;

namespace Alti.Domain.Interfaces.Repositories;

public interface ISeasonRepository
{
    Task<Season?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Season?> GetActiveForDateAsync(DateOnly date, CancellationToken ct = default);
    Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken ct = default);
    Task<bool> HasOverlapAsync(DateOnly startDate, DateOnly endDate, int? excludeSeasonId = null, CancellationToken ct = default);
    Task AddAsync(Season season, CancellationToken ct = default);
    void Update(Season season);
}