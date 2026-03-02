namespace Alti.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; internal set; }
    public DateTimeOffset CreatedAt { get; internal set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; internal set; } = DateTimeOffset.UtcNow;
}