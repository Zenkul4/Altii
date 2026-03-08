using Alti.Domain.Enums;

namespace Application.DTOs.Payment;

public class PaymentResponseDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string? ExternalReference { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}