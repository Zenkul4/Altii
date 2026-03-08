using Alti.Domain.Enums;

namespace Application.DTOs.Rate;

public class RateResponseDto
{
    public int Id { get; set; }
    public int SeasonId { get; set; }
    public RoomType RoomType { get; set; }
    public decimal PricePerNight { get; set; }
    public int CreatedById { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}