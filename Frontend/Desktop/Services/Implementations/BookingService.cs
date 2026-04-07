using Desktop.Models.Booking;
using Desktop.Services.Interfaces;

namespace Desktop.Services.Implementations;

public class BookingService : IBookingService
{
    private readonly ApiClient _api;
    public BookingService(ApiClient api) => _api = api;

    public Task<List<BookingDto>> GetAllAsync(int page = 1, int pageSize = 50)        // ← nueva
        => _api.GetAsync<List<BookingDto>>($"Bookings/all?page={page}&pageSize={pageSize}");

    public Task<List<BookingDto>> GetActiveAsync(int page = 1, int pageSize = 50)
        => _api.GetAsync<List<BookingDto>>($"Bookings/active?page={page}&pageSize={pageSize}");

    public Task<BookingDto> GetByIdAsync(int id)
        => _api.GetAsync<BookingDto>($"Bookings/{id}");

    public Task<BookingDto> GetByCodeAsync(string code)
        => _api.GetAsync<BookingDto>($"Bookings/code/{code}");

    public Task ConfirmAsync(int id, int paymentId)
        => _api.PatchAsync($"Bookings/{id}/confirm?paymentId={paymentId}");

    public Task CheckInAsync(int id, int receptionistId)
        => _api.PatchAsync($"Bookings/{id}/checkin?receptionistId={receptionistId}");

    public Task CheckOutAsync(int id)
        => _api.PatchAsync($"Bookings/{id}/checkout");

    public Task CancelAsync(int id)
        => _api.PatchAsync($"Bookings/{id}/cancel");
}