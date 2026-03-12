using Application.Dtos.Payment;

namespace Application.Services.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponseDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PaymentResponseDto>> GetByBookingAsync(int bookingId, CancellationToken ct = default);
    Task<PaymentResponseDto> CreateAsync(CreatePaymentDto dto, CancellationToken ct = default);
    Task<PaymentResponseDto> ApproveAsync(int id, string externalReference, CancellationToken ct = default);
    Task<PaymentResponseDto> RejectAsync(int id, CancellationToken ct = default);
    Task<PaymentResponseDto> RefundAsync(int id, CancellationToken ct = default);
}