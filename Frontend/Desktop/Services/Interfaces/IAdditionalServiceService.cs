using Desktop.Models.AdditionalService;

namespace Desktop.Services.Interfaces;

public interface IAdditionalServiceService
{
    Task<List<AdditionalServiceDto>> GetAllActiveAsync();
    Task<AdditionalServiceDto> CreateAsync(CreateAdditionalServiceDto dto);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task<List<BookingServiceDto>> GetByBookingAsync(int bookingId);
    Task<BookingServiceDto> AddToBookingAsync(CreateBookingServiceDto dto);
    Task<List<AdditionalServiceDto>> GetAllAsync();
    Task UpdateAsync(int id, decimal price, string? description);
}