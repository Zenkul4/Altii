using Alti.Domain.Entities;

namespace Alti.Domain.Services.Interfaces;

public interface IAdditionalServiceDomainService
{
    void UpdatePrice(AdditionalService service, decimal newPrice);
    void Deactivate(AdditionalService service);
    void Activate(AdditionalService service);
}