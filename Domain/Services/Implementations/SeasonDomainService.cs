using Alti.Domain.Entities;
using Alti.Domain.Services.Interfaces;

namespace Alti.Domain.Services.Implementations;

public class SeasonDomainService : ISeasonDomainService
{
    public void Update(Season season, string name, DateOnly startDate, DateOnly endDate, decimal multiplier, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Season name cannot be empty.", nameof(name));

        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date.", nameof(endDate));

        if (multiplier <= 0)
            throw new ArgumentException("Multiplier must be greater than zero.", nameof(multiplier));

        season.Name = name.Trim();
        season.StartDate = startDate;
        season.EndDate = endDate;
        season.Multiplier = multiplier;
        season.Description = description?.Trim();
        season.UpdatedAt = DateTimeOffset.UtcNow;
    }
}