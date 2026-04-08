using Desktop.Models.AdditionalService;
using Desktop.Services.Interfaces;

namespace Desktop.Services.Implementations;

public class AdditionalServiceService : IAdditionalServiceService
{
    private readonly ApiClient _api;
    public AdditionalServiceService(ApiClient api) => _api = api;

    public Task<List<AdditionalServiceDto>> GetAllActiveAsync()
        => _api.GetAsync<List<AdditionalServiceDto>>("AdditionalServices");

    public Task<AdditionalServiceDto> CreateAsync(CreateAdditionalServiceDto dto)
        => _api.PostAsync<AdditionalServiceDto>("AdditionalServices", dto);

    public Task ActivateAsync(int id)
        => _api.PatchAsync($"AdditionalServices/{id}/activate");

    public Task DeactivateAsync(int id)
        => _api.PatchAsync($"AdditionalServices/{id}/deactivate");

    public Task<List<BookingServiceDto>> GetByBookingAsync(int bookingId)
        => _api.GetAsync<List<BookingServiceDto>>($"BookingServices/booking/{bookingId}");

    public Task<BookingServiceDto> AddToBookingAsync(CreateBookingServiceDto dto)
        => _api.PostAsync<BookingServiceDto>("BookingServices", dto);

    public Task<List<AdditionalServiceDto>> GetAllAsync()
    => _api.GetAsync<List<AdditionalServiceDto>>("AdditionalServices/all");

    public async Task UpdateAsync(int id, decimal price, string? description)
    {
        await _api.PutAsync<AdditionalServiceDto>($"AdditionalServices/{id}",
            new UpdateAdditionalServiceDto { Price = price, Description = description });
    }
}