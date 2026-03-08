using Application.DTOs.Rate;

namespace Application.Services.Interfaces;

public interface IRateService
{
    Task<RateResponseDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<RateResponseDto>> GetBySeasonAsync(int seasonId, CancellationToken ct = default);
    Task<RateResponseDto> CreateAsync(CreateRateDto dto, int createdById, CancellationToken ct = default);
    Task<RateResponseDto> UpdateAsync(int id, UpdateRateDto dto, CancellationToken ct = default);
}