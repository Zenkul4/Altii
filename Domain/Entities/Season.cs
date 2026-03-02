using Alti.Domain.Common;

namespace Alti.Domain.Entities;

public class Season : BaseEntity
{
    private Season() { }

    public string Name { get; internal set; } = string.Empty;
    public DateOnly StartDate { get; internal set; }
    public DateOnly EndDate { get; internal set; }
    public decimal Multiplier { get; internal set; } = 1.00m;
    public string? Description { get; internal set; }
    public int CreatedById { get; internal set; }
}