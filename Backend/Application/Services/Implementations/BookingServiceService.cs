using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Interfaces;
using Application.Dtos.BookingService;
using Application.DTOs.BookingService;
using Application.Mappers;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class BookingServiceService : IBookingServiceService
{
    private readonly IUnitOfWork _uow;
    private readonly IBookingServiceFactory _factory;

    public BookingServiceService(IUnitOfWork uow, IBookingServiceFactory factory)
    {
        _uow = uow;
        _factory = factory;
    }

    public async Task<IReadOnlyList<BookingServiceResponseDto>> GetByBookingAsync(int bookingId, CancellationToken ct = default)
    {
        var services = await _uow.BookingServices.GetByBookingAsync(bookingId, ct);
        return services.Select(BookingServiceMapper.ToDto).ToList();
    }

    public async Task<BookingServiceResponseDto> AddAsync(CreateBookingServiceDto dto, CancellationToken ct = default)
    {
        var service = await _uow.AdditionalServices.GetByIdAsync(dto.ServiceId, ct)
            ?? throw new KeyNotFoundException($"Service {dto.ServiceId} not found.");

        var bookingService = _factory.Create(
            dto.BookingId, dto.ServiceId,
            dto.RegisteredById, dto.Quantity,
            service.Price);

        await _uow.BookingServices.AddAsync(bookingService, ct);
        await _uow.SaveChangesAsync(ct);

        return BookingServiceMapper.ToDto(bookingService);
    }
}