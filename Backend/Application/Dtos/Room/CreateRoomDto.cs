using Alti.Domain.Enums;

namespace Application.DTOs.Room;

public class CreateRoomDto
{
    public string Number { get; set; } = string.Empty;
    public RoomType Type { get; set; }
    public short Floor { get; set; }
    public short Capacity { get; set; }
    public decimal BasePrice { get; set; }
    public string? Description { get; set; }
}