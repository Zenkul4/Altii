namespace Desktop.Models.Rate;

public class RateDto
{
    public int Id { get; set; }
    public int SeasonId { get; set; }
    public int RoomType { get; set; }
    public decimal PricePerNight { get; set; }
    public int CreatedById { get; set; }
    public string CreatedAt { get; set; } = string.Empty;

    public string RoomTypeName => RoomType switch
    {
        0 => "Single",
        1 => "Double",
        2 => "Suite",
        3 => "Family",
        4 => "Penthouse",
        _ => $"Tipo {RoomType}"
    };
}

public class CreateRateDto
{
    public int SeasonId { get; set; }
    public int RoomType { get; set; }
    public decimal PricePerNight { get; set; }
}

public class UpdateRateDto
{
    public decimal PricePerNight { get; set; }
}