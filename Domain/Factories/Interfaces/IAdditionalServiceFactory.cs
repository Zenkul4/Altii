using Alti.Domain.Entities;

namespace Alti.Domain.Factories.Interfaces;

public interface IAdditionalServiceFactory
{
    AdditionalService Create(string name, decimal price, string? description = null);
}