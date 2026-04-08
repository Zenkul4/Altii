namespace Desktop.Models.Season;

public class SeasonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public decimal Multiplier { get; set; }
    public string? Description { get; set; }
    public int CreatedById { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
}

public class CreateSeasonDto
{
    public string Name { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public decimal Multiplier { get; set; } = 1.00m;
    public string? Description { get; set; }
}