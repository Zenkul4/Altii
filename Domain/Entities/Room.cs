using Alti.Domain.Common;
using Alti.Domain.Enums;

namespace Alti.Domain.Entities;

public class Room : BaseEntity
{
    internal Room() { }

    public string Number { get; internal set; } = string.Empty;
    public RoomType Type { get; internal set; }
    public short Floor { get; internal set; }
    public short Capacity { get; internal set; }
    public decimal BasePrice { get; internal set; }
    public string? Description { get; internal set; }
    public RoomStatus Status { get; internal set; } = RoomStatus.Available;
    public int RowVersion { get; internal set; } = 0;
}