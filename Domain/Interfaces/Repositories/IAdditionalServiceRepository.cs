using Alti.Domain.Entities;

namespace Alti.Domain.Interfaces.Repositories;

public interface IAdditionalServiceRepository : IBaseRepository<AdditionalService>
{
    Task<IReadOnlyList<AdditionalService>> GetAllActiveAsync(CancellationToken ct = default);
}