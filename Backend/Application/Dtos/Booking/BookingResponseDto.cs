using Alti.Domain.Enums;

namespace Application.DTOs.Booking;

public class BookingResponseDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int GuestId { get; set; }
    public string GuestFullName { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int Nights { get; set; }
    public decimal PricePerNight { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}