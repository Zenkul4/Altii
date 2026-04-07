using Alti.Domain.Interfaces.Repositories;
using Application.DTOs.Booking;
using Application.Interfaces;
using Application.Mappers;

namespace Application.Services.Implementations;

public class BookingAdminService : IBookingAdminService
{
    private readonly IBookingAdminRepository _repo;

    public BookingAdminService(IBookingAdminRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<BookingResponseDto>> GetAllBookingsAsync(
        int page, int pageSize, CancellationToken ct = default)
    {
        var bookings = await _repo.GetAllAsync(page, pageSize, ct);
        return bookings.Select(BookingMapper.ToDto).ToList();
    }
}