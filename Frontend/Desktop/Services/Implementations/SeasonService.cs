using Desktop.Models.Season;
using Desktop.Services.Interfaces;

namespace Desktop.Services.Implementations;

public class SeasonService : ISeasonService
{
    private readonly ApiClient _api;
    public SeasonService(ApiClient api) => _api = api;

    public Task<List<SeasonDto>> GetAllAsync()
        => _api.GetAsync<List<SeasonDto>>("Seasons");

    public async Task<SeasonDto> CreateAsync(CreateSeasonDto dto, int createdById)
        => await _api.PostAsync<SeasonDto>($"Seasons?createdById={createdById}", dto);

    public Task<SeasonDto> UpdateAsync(int id, CreateSeasonDto dto)
    => _api.PutAsync<SeasonDto>($"Seasons/{id}", dto);
}