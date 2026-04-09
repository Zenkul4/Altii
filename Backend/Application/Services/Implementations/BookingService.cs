using Alti.Domain.Exceptions;
using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Interfaces;
using Alti.Domain.Services.Implementations;
using Alti.Domain.Services.Interfaces;
using Application.DTOs.Booking;
using Application.Interfaces;
using Application.Mappers;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _uow;
    private readonly IBookingFactory _factory;
    private readonly IBookingDomainService _domainService;
    private readonly IPaymentDomainService _paymentDomainService;
    private readonly IRoomDomainService _roomDomainService;
    private readonly ICodeGeneratorService _codeGenerator;

    public BookingService(
        IUnitOfWork uow,
        IBookingFactory factory,
        IBookingDomainService domainService,
        IPaymentDomainService paymentDomainService,
        IRoomDomainService roomDomainService,
        ICodeGeneratorService codeGenerator)
    {
        _uow = uow;
        _factory = factory;
        _domainService = domainService;
        _paymentDomainService = paymentDomainService;
        _roomDomainService = roomDomainService;
        _codeGenerator = codeGenerator;
    }

    public async Task<BookingResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var booking = await _uow.Bookings.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Booking {id} not found.");

        return BookingMapper.ToDto(booking);
    }

    public async Task<BookingResponseDto> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        var booking = await _uow.Bookings.GetByCodeAsync(code, ct)
            ?? throw new KeyNotFoundException($"Booking {code} not found.");

        return BookingMapper.ToDto(booking);
    }

    public async Task<IReadOnlyList<BookingResponseDto>> GetByGuestAsync(
        int guestId, int page, int pageSize, CancellationToken ct = default)
    {
        var bookings = await _uow.Bookings.GetByGuestAsync(guestId, page, pageSize, ct);
        return bookings.Select(BookingMapper.ToDto).ToList();
    }

    public async Task<IReadOnlyList<BookingResponseDto>> GetActiveAsync(
        int page, int pageSize, CancellationToken ct = default)
    {
        var bookings = await _uow.Bookings.GetActiveAsync(page, pageSize, ct);
        return bookings.Select(BookingMapper.ToDto).ToList();
    }

    public async Task<BookingResponseDto> CreateAsync(CreateBookingDto dto, CancellationToken ct = default)
    {
        var room = await _uow.Rooms.GetByIdAsync(dto.RoomId, ct)
            ?? throw new KeyNotFoundException($"Room {dto.RoomId} not found.");

        if (await _uow.Bookings.HasConflictAsync(dto.RoomId, dto.CheckInDate, dto.CheckOutDate, null, ct))
            throw new DoubleBookingException(dto.RoomId, dto.CheckInDate, dto.CheckOutDate);

        var season = await _uow.Seasons.GetActiveForDateAsync(dto.CheckInDate, ct);
        var pricePerNight = season is not null
            ? room.BasePrice * season.Multiplier
            : room.BasePrice;

        await _uow.BeginTransactionAsync(ct);

        try
        {
            _roomDomainService.Block(room);
            _uow.Rooms.Update(room);

            var code = _codeGenerator.GenerateBookingCode();
            var booking = _factory.Create(
                code, dto.GuestId, dto.RoomId,
                dto.CheckInDate, dto.CheckOutDate,
                pricePerNight, dto.AttendedById, dto.Notes);

            await _uow.Bookings.AddAsync(booking, ct);
            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);

            return BookingMapper.ToDto(booking);
        }
        catch
        {
            await _uow.RollbackTransactionAsync(ct);
            throw;
        }
    }

    public async Task ConfirmAsync(int id, int paymentId, CancellationToken ct = default)
    {
        var booking = await _uow.Bookings.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Booking {id} not found.");

        var payment = await _uow.Payments.GetByIdAsync(paymentId, ct)
            ?? throw new KeyNotFoundException($"Payment {paymentId} not found.");

        _domainService.Confirm(booking, payment);
        _uow.Bookings.Update(booking);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task CheckInAsync(int id, int receptionistId, CancellationToken ct = default)
    {
        var booking = await _uow.Bookings.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Booking {id} not found.");

        var room = await _uow.Rooms.GetByIdAsync(booking.RoomId, ct)
            ?? throw new KeyNotFoundException($"Room {booking.RoomId} not found.");

        await _uow.BeginTransactionAsync(ct);

        try
        {
            _domainService.RegisterCheckIn(booking, receptionistId);
            _roomDomainService.MarkAsOccupied(room);
            _uow.Bookings.Update(booking);
            _uow.Rooms.Update(room);
            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);
        }
        catch
        {
            await _uow.RollbackTransactionAsync(ct);
            throw;
        }
    }

    public async Task CheckOutAsync(int id, CancellationToken ct = default)
    {
        var booking = await _uow.Bookings.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Booking {id} not found.");

        var room = await _uow.Rooms.GetByIdAsync(booking.RoomId, ct)
            ?? throw new KeyNotFoundException($"Room {booking.RoomId} not found.");

        await _uow.BeginTransactionAsync(ct);

        try
        {
            _domainService.RegisterCheckOut(booking);
            _roomDomainService.SendToCleaning(room);
            _uow.Bookings.Update(booking);
            _uow.Rooms.Update(room);
            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);
        }
        catch
        {
            await _uow.RollbackTransactionAsync(ct);
            throw;
        }
    }
    public async Task ExpireOverdueAsync(CancellationToken ct = default)
    {
        var expired = await _uow.Bookings.GetExpiredPendingAsync(ct);

        foreach (var booking in expired)
        {
            var room = await _uow.Rooms.GetByIdAsync(booking.RoomId, ct);

            _domainService.Expire(booking);
            _uow.Bookings.Update(booking);

            if (room is not null)
            {
                _roomDomainService.ReleaseBlock(room);
                _uow.Rooms.Update(room);
            }
        }

        if (expired.Count > 0)
            await _uow.SaveChangesAsync(ct);
    }

    public async Task CancelAsync(int id, CancellationToken ct = default)
    {
        var booking = await _uow.Bookings.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Booking {id} not found.");

        var room = await _uow.Rooms.GetByIdAsync(booking.RoomId, ct)
            ?? throw new KeyNotFoundException($"Room {booking.RoomId} not found.");

        await _uow.BeginTransactionAsync(ct);

        try
        {
            _domainService.Cancel(booking);
            _uow.Bookings.Update(booking);

            var payments = await _uow.Payments.GetByBookingAsync(booking.Id, ct);
            foreach (var payment in payments)
            {
                if (payment.Status == Alti.Domain.Enums.PaymentStatus.Pending)
                {
                    _paymentDomainService.Reject(payment);
                    _uow.Payments.Update(payment);
                }
            }

            if (room.Status == Alti.Domain.Enums.RoomStatus.Blocked)
            {
                _roomDomainService.ReleaseBlock(room);
                _uow.Rooms.Update(room);
            }

            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);
        }
        catch
        {
            await _uow.RollbackTransactionAsync(ct);
            throw;
        }
    }

    public async Task<BookingResponseDto> CreateByTypeAsync(CreateBookingByTypeDto dto, CancellationToken ct = default)
    {
        var availableRooms = await _uow.Rooms.GetAvailableAsync(
            dto.CheckInDate, dto.CheckOutDate, dto.RoomType, null, ct);

        if (availableRooms.Count == 0)
            throw new InvalidOperationException(
                $"No hay habitaciones disponibles de tipo {dto.RoomType} para las fechas seleccionadas.");

        var room = availableRooms.First();

        var createDto = new CreateBookingDto
        {
            GuestId = dto.GuestId,
            RoomId = room.Id,
            CheckInDate = dto.CheckInDate,
            CheckOutDate = dto.CheckOutDate,
            Notes = dto.Notes,
            AttendedById = null
        };

        return await CreateAsync(createDto, ct);
    }

    public async Task<decimal> GetExpectedTotalAsync(int bookingId, CancellationToken ct = default)
    {
        var booking = await _uow.Bookings.GetByIdAsync(bookingId, ct)
            ?? throw new KeyNotFoundException($"Booking {bookingId} not found.");

        var bookingServices = await _uow.BookingServices.GetByBookingAsync(bookingId, ct);
        var servicesTotal = bookingServices.Sum(bs => bs.UnitPrice * bs.Quantity);

        return booking.TotalPrice + servicesTotal;
    }
}