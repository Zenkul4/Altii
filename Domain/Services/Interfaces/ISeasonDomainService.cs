using Alti.Domain.Entities;

namespace Alti.Domain.Services.Interfaces;

public interface ISeasonDomainService
{
    void Update(Season season, string name, DateOnly startDate, DateOnly endDate, decimal multiplier, string? description = null);
}