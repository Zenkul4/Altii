using Alti.Domain.Enums;

namespace Application.DTOs.Rate;

public class CreateRateDto
{
    public int SeasonId { get; set; }
    public RoomType RoomType { get; set; }
    public decimal PricePerNight { get; set; }
}