namespace Application.DTOs.Payment;

public class UpdatePaymentDto
{
    public string ExternalReference { get; set; } = string.Empty;
    public bool Approved { get; set; }
}