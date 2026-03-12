using Alti.Domain.Entities;
using Application.Dtos.Payment;

namespace Application.Mappers;

public static class PaymentMapper
{
    public static PaymentResponseDto ToDto(Payment payment) => new()
    {
        Id = payment.Id,
        BookingId = payment.BookingId,
        Amount = payment.Amount,
        Status = payment.Status,
        ExternalReference = payment.ExternalReference,
        PaymentMethod = payment.PaymentMethod,
        ProcessedAt = payment.ProcessedAt,
        CreatedAt = payment.CreatedAt
    };
}