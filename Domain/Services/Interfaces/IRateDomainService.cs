using Alti.Domain.Entities;

namespace Alti.Domain.Services.Interfaces;

public interface IRateDomainService
{
    void UpdatePrice(Rate rate, decimal newPrice);
}