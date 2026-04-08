using Desktop.Models.Season;

namespace Desktop.Services.Interfaces;

public interface ISeasonService
{
    Task<List<SeasonDto>> GetAllAsync();
    Task<SeasonDto> CreateAsync(CreateSeasonDto dto, int createdById);

    Task<SeasonDto> UpdateAsync(int id, CreateSeasonDto dto);
}