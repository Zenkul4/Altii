using Alti.Domain.Entities;
using Alti.Domain.Enums;

namespace Alti.Domain.Interfaces.Repositories;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog, CancellationToken ct = default);
    Task<IReadOnlyList<AuditLog>> GetByUserAsync(int userId, CancellationToken ct = default);
    Task<IReadOnlyList<AuditLog>> GetByEntityAsync(AuditEntity entity, int entityId, CancellationToken ct = default);
}