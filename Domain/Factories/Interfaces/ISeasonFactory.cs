using Alti.Domain.Entities;

namespace Alti.Domain.Factories.Interfaces;

public interface ISeasonFactory
{
    Season Create(
        string name,
        DateOnly startDate,
        DateOnly endDate,
        decimal multiplier,
        int createdById,
        string? description = null);
}