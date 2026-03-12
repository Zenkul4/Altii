using Alti.Domain.Common;

namespace Alti.Domain.Entities;

public class AdditionalService : BaseEntity
{
    internal AdditionalService() { }

    public string Name { get; internal set; } = string.Empty;
    public string? Description { get; internal set; }
    public decimal Price { get; internal set; }
    public bool IsActive { get; internal set; } = true;
}