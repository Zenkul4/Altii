using Alti.Domain.Enums;

namespace Alti.Domain.Entities;

public class AuditLog
{
    internal AuditLog() { }

    public long Id { get; internal set; }
    public int? UserId { get; internal set; }
    public UserRole? ExecutorRole { get; internal set; }
    public AuditAction Action { get; internal set; }
    public AuditEntity Entity { get; internal set; }
    public int? EntityId { get; internal set; }
    public string? PreviousData { get; internal set; }
    public string? NewData { get; internal set; }
    public string? IpAddress { get; internal set; }
    public string? Description { get; internal set; }
    public DateTimeOffset ExecutedAt { get; internal set; } = DateTimeOffset.UtcNow;
}