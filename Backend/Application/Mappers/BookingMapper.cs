using Alti.Domain.Entities;
using Application.Dtos.Booking;
using Application.DTOs.Booking;

namespace Application.Mappers;

public static class BookingMapper
{
    public static BookingResponseDto ToDto(Booking booking) => new()
    {
        Id = booking.Id,
        Code = booking.Code,
        GuestId = booking.GuestId,
        GuestFullName = string.Empty,
        RoomId = booking.RoomId,
        RoomNumber = string.Empty,
        CheckInDate = booking.CheckInDate,
        CheckOutDate = booking.CheckOutDate,
        Nights = booking.CheckOutDate.DayNumber - booking.CheckInDate.DayNumber,
        PricePerNight = booking.PricePerNight,
        TotalPrice = booking.TotalPrice,
        Status = booking.Status,
        ExpiresAt = booking.ExpiresAt,
        Notes = booking.Notes,
        CreatedAt = booking.CreatedAt
    };
}