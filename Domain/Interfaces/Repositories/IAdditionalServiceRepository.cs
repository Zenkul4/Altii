using Alti.Domain.Entities;

namespace Core.domain.Interfaces.Repositories;

public interface IAdditionalServiceRepository
{
    Task<AdditionalService?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<AdditionalService>> GetAllActiveAsync(CancellationToken ct = default);
    Task AddAsync(AdditionalService service, CancellationToken ct = default);
    void Update(AdditionalService service);
}