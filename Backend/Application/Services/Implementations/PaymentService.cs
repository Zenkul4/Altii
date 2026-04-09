using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Interfaces;
using Alti.Domain.Services.Interfaces;
using Application.Dtos.Payment;
using Application.Mappers;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _uow;
    private readonly IPaymentFactory _factory;
    private readonly IPaymentDomainService _domainService;
    private readonly IBookingDomainService _bookingDomainService;
    private readonly IRoomDomainService _roomDomainService;

    public PaymentService(
        IUnitOfWork uow,
        IPaymentFactory factory,
        IPaymentDomainService domainService,
        IBookingDomainService bookingDomainService,
        IRoomDomainService roomDomainService)
    {
        _uow = uow;
        _factory = factory;
        _domainService = domainService;
        _bookingDomainService = bookingDomainService;
        _roomDomainService = roomDomainService;
    }

    public async Task<PaymentResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var payment = await _uow.Payments.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Payment {id} not found.");

        return PaymentMapper.ToDto(payment);
    }

    public async Task<IReadOnlyList<PaymentResponseDto>> GetByBookingAsync(int bookingId, CancellationToken ct = default)
    {
        var payments = await _uow.Payments.GetByBookingAsync(bookingId, ct);
        return payments.Select(PaymentMapper.ToDto).ToList();
    }

    public async Task<PaymentResponseDto> CreateAsync(CreatePaymentDto dto, CancellationToken ct = default)
    {
        var booking = await _uow.Bookings.GetByIdAsync(dto.BookingId, ct)
            ?? throw new KeyNotFoundException($"Booking {dto.BookingId} not found.");

        if (booking.Status != Alti.Domain.Enums.BookingStatus.PendingPayment)
            throw new InvalidOperationException(
                $"Booking {dto.BookingId} is not pending payment (current: {booking.Status}).");

        var bookingServices = await _uow.BookingServices.GetByBookingAsync(booking.Id, ct);
        var servicesTotal = bookingServices.Sum(bs => bs.UnitPrice * bs.Quantity);
        var expectedTotal = booking.TotalPrice + servicesTotal;

        if (Math.Abs(dto.Amount - expectedTotal) > 0.01m)
            throw new ArgumentException(
                $"Payment amount ({dto.Amount}) does not match expected total ({expectedTotal}).");

        var payment = _factory.Create(dto.BookingId, dto.Amount, dto.PaymentMethod);

        await _uow.Payments.AddAsync(payment, ct);
        await _uow.SaveChangesAsync(ct);

        return PaymentMapper.ToDto(payment);
    }

    public async Task<PaymentResponseDto> ApproveAsync(int id, string externalReference, CancellationToken ct = default)
    {
        var payment = await _uow.Payments.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Payment {id} not found.");

        _domainService.Approve(payment, externalReference);
        _uow.Payments.Update(payment);
        await _uow.SaveChangesAsync(ct);

        return PaymentMapper.ToDto(payment);
    }

    public async Task<PaymentResponseDto> RejectAsync(int id, CancellationToken ct = default)
    {
        var payment = await _uow.Payments.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Payment {id} not found.");

        _domainService.Reject(payment);
        _uow.Payments.Update(payment);
        await _uow.SaveChangesAsync(ct);

        return PaymentMapper.ToDto(payment);
    }

    public async Task<PaymentResponseDto> RefundAsync(int id, CancellationToken ct = default)
    {
        var payment = await _uow.Payments.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Payment {id} not found.");

        _domainService.Refund(payment);
        _uow.Payments.Update(payment);

        var booking = await _uow.Bookings.GetByIdAsync(payment.BookingId, ct);
        if (booking is not null &&
            booking.Status is Alti.Domain.Enums.BookingStatus.Confirmed
                           or Alti.Domain.Enums.BookingStatus.PendingPayment)
        {
            _bookingDomainService.Cancel(booking);
            _uow.Bookings.Update(booking);

            var room = await _uow.Rooms.GetByIdAsync(booking.RoomId, ct);
            if (room is not null && room.Status == Alti.Domain.Enums.RoomStatus.Blocked)
            {
                _roomDomainService.ReleaseBlock(room);
                _uow.Rooms.Update(room);
            }
        }

        await _uow.SaveChangesAsync(ct);

        return PaymentMapper.ToDto(payment);
    }

    public async Task<PaymentResponseDto> RegisterCashPaymentAsync(int bookingId, CancellationToken ct = default)
    {
        var booking = await _uow.Bookings.GetByIdAsync(bookingId, ct)
            ?? throw new KeyNotFoundException($"Booking {bookingId} not found.");

        if (booking.Status != Alti.Domain.Enums.BookingStatus.PendingPayment)
            throw new InvalidOperationException(
                $"Booking {bookingId} is not pending payment (current: {booking.Status}).");

        await _uow.BeginTransactionAsync(ct);

        try
        {
            var payment = _factory.Create(bookingId, booking.TotalPrice, "Cash");
            await _uow.Payments.AddAsync(payment, ct);
            await _uow.SaveChangesAsync(ct);

            _domainService.Approve(payment, $"CASH-{DateTime.UtcNow:yyyyMMddHHmmss}");
            _uow.Payments.Update(payment);

            _bookingDomainService.Confirm(booking, payment);
            _uow.Bookings.Update(booking);

            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);

            return PaymentMapper.ToDto(payment);
        }
        catch
        {
            await _uow.RollbackTransactionAsync(ct);
            throw;
        }
    }
}