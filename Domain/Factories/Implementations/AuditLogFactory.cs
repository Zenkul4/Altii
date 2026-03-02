using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Factories.Interfaces;

namespace Alti.Domain.Factories.Implementations;

public class AuditLogFactory : IAuditLogFactory
{
    public AuditLog Create(
        AuditAction action,
        AuditEntity entity,
        int? entityId = null,
        int? userId = null,
        UserRole? executorRole = null,
        string? previousData = null,
        string? newData = null,
        string? ipAddress = null,
        string? description = null)
    {
        return new AuditLog
        {
            Action = action,
            Entity = entity,
            EntityId = entityId,
            UserId = userId,
            ExecutorRole = executorRole,
            PreviousData = previousData,
            NewData = newData,
            IpAddress = ipAddress,
            Description = description
        };
    }
}