using Desktop.Models.Payment;
using Desktop.Services.Interfaces;

namespace Desktop.Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly ApiClient _api;
    public PaymentService(ApiClient api) => _api = api;

    public Task<List<PaymentDto>> GetByBookingAsync(int bookingId)
        => _api.GetAsync<List<PaymentDto>>($"Payments/booking/{bookingId}");

    public Task<PaymentDto> ApproveAsync(int id, string externalReference)
        => _api.PatchAsync<PaymentDto>($"Payments/{id}/approve", $"externalReference={Uri.EscapeDataString(externalReference)}");

    public Task<PaymentDto> RejectAsync(int id)
        => _api.PatchAsync<PaymentDto>($"Payments/{id}/reject");

    public Task<PaymentDto> RefundAsync(int id)
        => _api.PatchAsync<PaymentDto>($"Payments/{id}/refund");

    public async Task RegisterCashPaymentAsync(int bookingId)
    {
        await _api.PostAsync<object>($"Payments/cash/{bookingId}", new { });
    }
}