using Alti.Domain.Entities;
using Alti.Domain.Interfaces.Repositories;
using Core.domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class AdditionalServiceRepository : IAdditionalServiceRepository
{
    private readonly AppDbContext _context;

    public AdditionalServiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AdditionalService?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.AdditionalServices.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IReadOnlyList<AdditionalService>> GetAllActiveAsync(CancellationToken ct = default)
        => await _context.AdditionalServices.Where(s => s.IsActive).ToListAsync(ct);

    public async Task AddAsync(AdditionalService service, CancellationToken ct = default)
        => await _context.AdditionalServices.AddAsync(service, ct);

    public void Update(AdditionalService service)
        => _context.AdditionalServices.Update(service);
}