using Alti.Domain.Entities;
using Application.DTOs.BookingService;

namespace Application.Mappers;

public static class BookingServiceMapper
{
    public static BookingServiceResponseDto ToDto(BookingService bs) => new()
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