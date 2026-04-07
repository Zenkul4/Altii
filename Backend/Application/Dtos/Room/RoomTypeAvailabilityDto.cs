using Alti.Domain.Enums;

namespace Application.DTOs.Room;

public class RoomTypeAvailabilityDto
{
    public RoomType Type { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public int TotalRooms { get; set; }
    public int AvailableRooms { get; set; }
    public short MaxCapacity { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public string? SampleDescription { get; set; }
}