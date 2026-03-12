using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class RateRepository : BaseRepository<Rate>, IRateRepository
{
    public RateRepository(AppDbContext context) : base(context) { }

    public async Task<Rate?> GetBySeasonAndTypeAsync(int seasonId, RoomType roomType, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(r => r.SeasonId == seasonId && r.RoomType == roomType, ct);

    public async Task<IReadOnlyList<Rate>> GetBySeasonAsync(int seasonId, CancellationToken ct = default)
        => await DbSet.Where(r => r.SeasonId == seasonId).ToListAsync(ct);
}