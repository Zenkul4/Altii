using Desktop.Models.Rate;
using Desktop.Services.Interfaces;

namespace Desktop.Services.Implementations;

public class RateService : IRateService
{
    private readonly ApiClient _api;
    public RateService(ApiClient api) => _api = api;

    public Task<List<RateDto>> GetBySeasonAsync(int seasonId)
        => _api.GetAsync<List<RateDto>>($"Rates/season/{seasonId}");

    public async Task<RateDto> CreateAsync(CreateRateDto dto, int createdById)
        => await _api.PostAsync<RateDto>($"Rates?createdById={createdById}", dto);

    public Task<RateDto> UpdateAsync(int id, decimal pricePerNight)
        => _api.PutAsync<RateDto>($"Rates/{id}", new UpdateRateDto { PricePerNight = pricePerNight });
}