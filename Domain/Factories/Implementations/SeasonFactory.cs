using Alti.Domain.Entities;
using Alti.Domain.Factories.Interfaces;

namespace Alti.Domain.Factories.Implementations;

public class SeasonFactory : ISeasonFactory
{
    public Season Create(
        string name,
        DateOnly startDate,
        DateOnly endDate,
        decimal multiplier,
        int createdById,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Season name cannot be empty.", nameof(name));

        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date.", nameof(endDate));

        if (multiplier <= 0)
            throw new ArgumentException("Multiplier must be greater than zero.", nameof(multiplier));

        return new Season
        {
            Name = name.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            Multiplier = multiplier,
            CreatedById = createdById,
            Description = description?.Trim()
        };
    }
}