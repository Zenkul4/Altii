using Alti.Domain.Enums;

namespace Application.DTOs.Booking;

public class CreateBookingByTypeDto
{
    public int GuestId { get; set; }
    public RoomType RoomType { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public string? Notes { get; set; }
}