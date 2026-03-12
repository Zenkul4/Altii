using Alti.Domain.Entities;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class SeasonRepository : BaseRepository<Season>, ISeasonRepository
{
    public SeasonRepository(AppDbContext context) : base(context) { }

    public async Task<Season?> GetActiveForDateAsync(DateOnly date, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(s => s.StartDate <= date && s.EndDate >= date, ct);

    public async Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken ct = default)
        => await DbSet.OrderBy(s => s.StartDate).ToListAsync(ct);

    public async Task<bool> HasOverlapAsync(
        DateOnly startDate,
        DateOnly endDate,
        int? excludeSeasonId = null,
        CancellationToken ct = default)
    {
        var query = DbSet.Where(s => s.StartDate <= endDate && s.EndDate >= startDate);

        if (excludeSeasonId.HasValue)
            query = query.Where(s => s.Id != excludeSeasonId.Value);

        return await query.AnyAsync(ct);
    }
}