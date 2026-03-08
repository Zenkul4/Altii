using Application.DTOs.AdditionalService;

namespace Application.Services.Interfaces;

public interface IAdditionalServiceService
{
    Task<AdditionalServiceResponseDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<AdditionalServiceResponseDto>> GetAllActiveAsync(CancellationToken ct = default);
    Task<AdditionalServiceResponseDto> CreateAsync(CreateAdditionalServiceDto dto, CancellationToken ct = default);
    Task<AdditionalServiceResponseDto> UpdateAsync(int id, UpdateAdditionalServiceDto dto, CancellationToken ct = default);
    Task DeactivateAsync(int id, CancellationToken ct = default);
    Task ActivateAsync(int id, CancellationToken ct = default);
}