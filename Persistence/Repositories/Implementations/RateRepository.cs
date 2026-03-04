using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class RateRepository : IRateRepository
{
    private readonly AppDbContext _context;

    public RateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Rate?> GetBySeasonAndTypeAsync(int seasonId, RoomType roomType, CancellationToken ct = default)
        => await _context.Rates.FirstOrDefaultAsync(r => r.SeasonId == seasonId && r.RoomType == roomType, ct);

    public async Task<IReadOnlyList<Rate>> GetBySeasonAsync(int seasonId, CancellationToken ct = default)
        => await _context.Rates.Where(r => r.SeasonId == seasonId).ToListAsync(ct);

    public async Task AddAsync(Rate rate, CancellationToken ct = default)
        => await _context.Rates.AddAsync(rate, ct);

    public void Update(Rate rate)
        => _context.Rates.Update(rate);
}