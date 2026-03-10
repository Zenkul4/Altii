namespace Application.DTOs.Season;

public class CreateSeasonDto
{
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Multiplier { get; set; } = 1.00m;
    public string? Description { get; set; }
}