using Alti.Domain.Interfaces;
using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Services.Interfaces;
using Alti.Domain.Exceptions;
using Application.DTOs.Booking;
using Application.Services.Interfaces;
using Infrastructure.Services.Interfaces;

namespace Application.Services.Implementations;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _uow;
    private readonly IBookingFactory _factory;
    private readonly IBookingDomainService _domainService;
    private readonly IRoomDomainService _roomDomainService;
    private readonly ICodeGeneratorService _codeGenerator;

    public BookingService(
        IUnitOfWork uow,
        IBookingFactory factory,
        IBookingDomainService domainService,
        IRoomDomainService roomDomainService,
        ICodeGeneratorService codeGenerator)
    {
        _uow = uow;
        _factory = factory;
        _domainService = domainService;
        _roomDomainService = roomDomainService;
        _codeGenerator = codeGenerator;
    }

    public async Task<BookingResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var booking = await _uow.Bookings.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Booking {id} not found.");

        return MapToResponse(booking);
    }

    public async Task<BookingResponseDto> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        var booking = await _uow.Bookings.GetByCodeAsync(code, ct)
            ?? throw new KeyNotFoundException($"Booking with code {code} not found.");

        return MapToResponse(booking);
    }

    public async Task<IReadOnlyList<BookingResponseDto>> GetByGuestAsync(int guestId, CancellationToken ct = default)
    {
        var bookings = await _uow.Bookings.GetByGuestAsync(guestId, ct);
        return bookings.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<BookingResponseDto>> GetActiveAsync(CancellationToken ct = default)
    {
        var bookings = await _uow.Bookings.GetActiveAsync(ct);
        return bookings.Select(MapToResponse).ToList();
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
                code,
                dto.GuestId,
                dto.RoomId,
                dto.CheckInDate,
                dto.CheckOutDate,
                pricePerNight,
                dto.AttendedById,
                dto.Notes);

            await _uow.Bookings.AddAsync(booking, ct);
            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);

            return MapToResponse(booking);
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
            _roomDomainService.ReleaseBlock(room);

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

    private static BookingResponseDto MapToResponse(Alti.Domain.Entities.Booking booking) => new()
    {
        Id = booking.Id,
        Code = booking.Code,
        GuestId = booking.GuestId,
        GuestFullName = string.Empty,
        RoomId = booking.RoomId,
        RoomNumber = string.Empty,
        CheckInDate = booking.CheckInDate,
        CheckOutDate = booking.CheckOutDate,
        Nights = booking.CheckOutDate.DayNumber - booking.CheckInDate.DayNumber,
        PricePerNight = booking.PricePerNight,
        TotalPrice = booking.TotalPrice,
        Status = booking.Status,
        ExpiresAt = booking.ExpiresAt,
        Notes = booking.Notes,
        CreatedAt = booking.CreatedAt
    };
}