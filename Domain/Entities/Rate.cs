using Alti.Domain.Common;
using Alti.Domain.Enums;

namespace Alti.Domain.Entities;

public class Rate : BaseEntity
{
    private Rate() { }

    public int SeasonId { get; internal set; }
    public RoomType RoomType { get; internal set; }
    public decimal PricePerNight { get; internal set; }
    public int CreatedById { get; internal set; }
}