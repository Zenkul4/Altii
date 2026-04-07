using Application.DTOs.Booking;

namespace Application.Interfaces;

public interface IBookingAdminService
{
    Task<IReadOnlyList<BookingResponseDto>> GetAllBookingsAsync(int page, int pageSize, CancellationToken ct = default);
}