using Alti.Domain.Enums;

namespace Application.DTOs.Room;

public class RoomResponseDto
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public RoomType Type { get; set; }
    public short Floor { get; set; }
    public short Capacity { get; set; }
    public decimal BasePrice { get; set; }
    public string? Description { get; set; }
    public RoomStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
