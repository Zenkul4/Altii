using Application.DTOs.Booking;

namespace Application.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponseDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<BookingResponseDto> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<BookingResponseDto>> GetByGuestAsync(int guestId, int page, int pageSize, CancellationToken ct = default);
    Task<IReadOnlyList<BookingResponseDto>> GetActiveAsync(int page, int pageSize, CancellationToken ct = default);
    Task<BookingResponseDto> CreateAsync(CreateBookingDto dto, CancellationToken ct = default);
    Task ConfirmAsync(int id, int paymentId, CancellationToken ct = default);
    Task CheckInAsync(int id, int receptionistId, CancellationToken ct = default);
    Task CheckOutAsync(int id, CancellationToken ct = default);
    Task CancelAsync(int id, CancellationToken ct = default);
    Task ExpireOverdueAsync(CancellationToken ct = default);
}