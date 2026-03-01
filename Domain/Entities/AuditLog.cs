using Alti.Domain.Enums;
using Core.domain.entities;

namespace Alti.Domain.Entities;

public class AuditLog
{
    private AuditLog() { }

    public long Id { get; private set; }
    public int? UserId { get; private set; }
    public UserRole? ExecutorRole { get; private set; }
    public AuditAction Action { get; private set; }
    public AuditEntity Entity { get; private set; }
    public int? EntityId { get; private set; }
    public string? PreviousData { get; private set; }
    public string? NewData { get; private set; }
    public string? IpAddress { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset ExecutedAt { get; private set; } = DateTimeOffset.UtcNow;

    public User? User { get; private set; }

    public static AuditLog Register(
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