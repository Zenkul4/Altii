using Alti.Domain.Interfaces;
using Alti.Domain.Factories.Interfaces;
using Application.DTOs.BookingService;
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
        return services.Select(MapToResponse).ToList();
    }

    public async Task<BookingServiceResponseDto> AddAsync(CreateBookingServiceDto dto, CancellationToken ct = default)
    {
        var service = await _uow.AdditionalServices.GetByIdAsync(dto.ServiceId, ct)
            ?? throw new KeyNotFoundException($"Service {dto.ServiceId} not found.");

        var bookingService = _factory.Create(
            dto.BookingId,
            dto.ServiceId,
            dto.RegisteredById,
            dto.Quantity,
            service.Price);

        await _uow.BookingServices.AddAsync(bookingService, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToResponse(bookingService);
    }

    private static BookingServiceResponseDto MapToResponse(Alti.Domain.Entities.BookingService bs) => new()
    {
        Id = bs.Id,
        BookingId = bs.BookingId,
        ServiceId = bs.ServiceId,
        ServiceName = string.Empty,
        Quantity = bs.Quantity,
        UnitPrice = bs.UnitPrice,
        Subtotal = bs.Quantity * bs.UnitPrice,
        RegisteredAt = bs.RegisteredAt
    };
}