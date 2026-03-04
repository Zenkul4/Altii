using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _context;

    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog auditLog, CancellationToken ct = default)
        => await _context.AuditLogs.AddAsync(auditLog, ct);

    public async Task<IReadOnlyList<AuditLog>> GetByUserAsync(int userId, CancellationToken ct = default)
        => await _context.AuditLogs
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.ExecutedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<AuditLog>> GetByEntityAsync(AuditEntity entity, int entityId, CancellationToken ct = default)
        => await _context.AuditLogs
            .Where(a => a.Entity == entity && a.EntityId == entityId)
            .OrderByDescending(a => a.ExecutedAt)
            .ToListAsync(ct);
}