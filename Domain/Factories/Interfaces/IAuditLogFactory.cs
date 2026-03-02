using Alti.Domain.Entities;
using Alti.Domain.Enums;

namespace Alti.Domain.Factories.Interfaces;

public interface IAuditLogFactory
{
    AuditLog Create(
        AuditAction action,
        AuditEntity entity,
        int? entityId = null,
        int? userId = null,
        UserRole? executorRole = null,
        string? previousData = null,
        string? newData = null,
        string? ipAddress = null,
        string? description = null);
}