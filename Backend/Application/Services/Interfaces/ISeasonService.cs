using Application.DTOs.Season;

namespace Application.Services.Interfaces;

public interface ISeasonService
{
    Task<SeasonResponseDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<SeasonResponseDto>> GetAllAsync(CancellationToken ct = default);
    Task<SeasonResponseDto> CreateAsync(CreateSeasonDto dto, int createdById, CancellationToken ct = default);
    Task<SeasonResponseDto> UpdateAsync(int id, UpdateSeasonDto dto, CancellationToken ct = default);
}