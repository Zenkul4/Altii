using Desktop.Models.Rate;

namespace Desktop.Services.Interfaces;

public interface IRateService
{
    Task<List<RateDto>> GetBySeasonAsync(int seasonId);
    Task<RateDto> CreateAsync(CreateRateDto dto, int createdById);

    Task<RateDto> UpdateAsync(int id, decimal pricePerNight);
}