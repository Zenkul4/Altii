using Alti.Domain.Entities;

namespace Alti.Domain.Interfaces.Repositories;

public interface IAdditionalServiceAdminRepository
{
    Task<IReadOnlyList<AdditionalService>> GetAllAsync(CancellationToken ct = default);
}