using Alti.Domain.Entities;
using Alti.Domain.Services.Interfaces;

namespace Alti.Domain.Services.Implementations;

public class AdditionalServiceDomainService : IAdditionalServiceDomainService
{
    public void UpdatePrice(AdditionalService service, decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(newPrice));

        service.Price = newPrice;
        service.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateDetails(AdditionalService service, decimal newPrice, string? newDescription)
    {
        if (newPrice < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(newPrice));

        service.Price = newPrice;
        service.Description = newDescription?.Trim();
        service.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate(AdditionalService service)
    {
        service.IsActive = false;
        service.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Activate(AdditionalService service)
    {
        service.IsActive = true;
        service.UpdatedAt = DateTimeOffset.UtcNow;
    }
}