using Alti.Domain.Entities;
using Alti.Domain.Factories.Interfaces;

namespace Alti.Domain.Factories.Implementations;

public class AdditionalServiceFactory : IAdditionalServiceFactory
{
    public AdditionalService Create(string name, decimal price, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Service name cannot be empty.", nameof(name));

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));

        return new AdditionalService
        {
            Name = name.Trim(),
            Price = price,
            Description = description?.Trim()
        };
    }
}