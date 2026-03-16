using Alti.Domain.Entities;

namespace Alti.Domain.Services.Interfaces;

public interface IAdditionalServiceDomainService
{
    void UpdatePrice(AdditionalService service, decimal newPrice);
    void UpdateDetails(AdditionalService service, decimal newPrice, string? newDescription);
    void Deactivate(AdditionalService service);
    void Activate(AdditionalService service);
}