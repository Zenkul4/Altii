using Alti.Domain.Entities;
using Alti.Domain.Services.Interfaces;

namespace Alti.Domain.Services.Implementations;

public class RateDomainService : IRateDomainService
{
    public void UpdatePrice(Rate rate, decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price must be greater than zero.", nameof(newPrice));

        rate.PricePerNight = newPrice;
        rate.UpdatedAt = DateTimeOffset.UtcNow;
    }
}