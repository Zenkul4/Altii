using Alti.Domain.Entities;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class SeasonRepository : ISeasonRepository
{
    private readonly AppDbContext _context;

    public SeasonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Season?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Seasons.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<Season?> GetActiveForDateAsync(DateOnly date, CancellationToken ct = default)
        => await _context.Seasons.FirstOrDefaultAsync(s => s.StartDate <= date && s.EndDate >= date, ct);

    public async Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken ct = default)
        => await _context.Seasons.OrderBy(s => s.StartDate).ToListAsync(ct);

    public async Task<bool> HasOverlapAsync(
        DateOnly startDate,
        DateOnly endDate,
        int? excludeSeasonId = null,
        CancellationToken ct = default)
    {
        var query = _context.Seasons
            .Where(s => s.StartDate <= endDate && s.EndDate >= startDate);

        if (excludeSeasonId.HasValue)
            query = query.Where(s => s.Id != excludeSeasonId.Value);

        return await query.AnyAsync(ct);
    }

    public async Task AddAsync(Season season, CancellationToken ct = default)
        => await _context.Seasons.AddAsync(season, ct);

    public void Update(Season season)
        => _context.Seasons.Update(season);
}