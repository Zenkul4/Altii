using Application.Dtos.BookingService;
using Application.DTOs.BookingService;

namespace Application.Services.Interfaces;

public interface IBookingServiceService
{
    Task<IReadOnlyList<BookingServiceResponseDto>> GetByBookingAsync(int bookingId, CancellationToken ct = default);
    Task<BookingServiceResponseDto> AddAsync(CreateBookingServiceDto dto, CancellationToken ct = default);
}