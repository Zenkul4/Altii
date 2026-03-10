namespace Application.DTOs.Season;

public class UpdateSeasonDto
{
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Multiplier { get; set; }
    public string? Description { get; set; }
}