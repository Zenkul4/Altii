using Alti.Domain.Entities;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class AdditionalServiceRepository : BaseRepository<AdditionalService>, IAdditionalServiceRepository
{
    public AdditionalServiceRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<AdditionalService>> GetAllActiveAsync(CancellationToken ct = default)
        => await DbSet.Where(s => s.IsActive).ToListAsync(ct);
}