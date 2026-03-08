using Alti.Domain.Interfaces;
using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Services.Interfaces;
using Application.DTOs.Payment;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _uow;
    private readonly IPaymentFactory _factory;
    private readonly IPaymentDomainService _domainService;

    public PaymentService(IUnitOfWork uow, IPaymentFactory factory, IPaymentDomainService domainService)
    {
        _uow = uow;
        _factory = factory;
        _domainService = domainService;
    }

    public async Task<PaymentResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var payment = await _uow.Payments.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Payment {id} not found.");

        return MapToResponse(payment);
    }

    public async Task<IReadOnlyList<PaymentResponseDto>> GetByBookingAsync(int bookingId, CancellationToken ct = default)
    {
        var payments = await _uow.Payments.GetByBookingAsync(bookingId, ct);
        return payments.Select(MapToResponse).ToList();
    }

    public async Task<PaymentResponseDto> CreateAsync(CreatePaymentDto dto, CancellationToken ct = default)
    {
        var payment = _factory.Create(dto.BookingId, dto.Amount, dto.PaymentMethod);

        await _uow.Payments.AddAsync(payment, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToResponse(payment);
    }

    public async Task<PaymentResponseDto> ApproveAsync(int id, string externalReference, CancellationToken ct = default)
    {
        var payment = await _uow.Payments.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Payment {id} not found.");

        _domainService.Approve(payment, externalReference);
        _uow.Payments.Update(payment);
        await _uow.SaveChangesAsync(ct);

        return MapToResponse(payment);
    }

    public async Task<PaymentResponseDto> RejectAsync(int id, CancellationToken ct = default)
    {
        var payment = await _uow.Payments.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Payment {id} not found.");

        _domainService.Reject(payment);
        _uow.Payments.Update(payment);
        await _uow.SaveChangesAsync(ct);

        return MapToResponse(payment);
    }

    public async Task<PaymentResponseDto> RefundAsync(int id, CancellationToken ct = default)
    {
        var payment = await _uow.Payments.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Payment {id} not found.");

        _domainService.Refund(payment);
        _uow.Payments.Update(payment);
        await _uow.SaveChangesAsync(ct);

        return MapToResponse(payment);
    }

    private static PaymentResponseDto MapToResponse(Alti.Domain.Entities.Payment payment) => new()
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