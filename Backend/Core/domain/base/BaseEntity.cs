namespace Alti.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; protected set; }

    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; protected set; } = DateTimeOffset.UtcNow;

    protected void RegisterUpdate()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}