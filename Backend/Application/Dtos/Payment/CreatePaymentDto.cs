namespace Application.DTOs.Payment;

public class CreatePaymentDto
{
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentMethod { get; set; }
}