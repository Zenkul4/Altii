using Desktop.Models.Payment;

namespace Desktop.Services.Interfaces;

public interface IPaymentService
{
    Task<List<PaymentDto>> GetByBookingAsync(int bookingId);
    Task<PaymentDto> ApproveAsync(int id, string externalReference);
    Task<PaymentDto> RejectAsync(int id);
    Task<PaymentDto> RefundAsync(int id);
    Task RegisterCashPaymentAsync(int bookingId);
}