using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Alti.Tests;

/// <summary>
/// US-21: Pruebas e implementación de InMemory.
/// Cubre: Task 24 (proveedor), Task 23 (nombre DB), Task 26 (sin PostgreSQL),
///        Task 25 (preparar datos de prueba), Task 27 (validar habitaciones y reservas).
/// </summary>
public class AppDbContextInMemoryTests : IDisposable
{
    private readonly AppDbContext _context;

    public AppDbContextInMemoryTests()
    {
        // Task 24: Configurar el proveedor InMemory
        // Task 23: Definir el nombre de la base de datos InMemory
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "AltiiDb_Tests_" + Guid.NewGuid())
            .Options;

        // Task 26: La app arranca sin PostgreSQL — se mockea IHttpContextAccessor
        var httpMock = new Mock<IHttpContextAccessor>();
        httpMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        _context = new AppDbContext(options, httpMock.Object);

        // Task 25: Preparar datos de prueba (seed)
        SeedTestData();
    }

    // ─────────────────────────────────────────────
    // Task 25: Preparar datos de prueba
    // ─────────────────────────────────────────────
    private void SeedTestData()
    {
        _context.Rooms.AddRange(
            new Room { Number = "101", Type = RoomType.Single,  Floor = 1, Capacity = 1, BasePrice = 80m,  Status = RoomStatus.Available },
            new Room { Number = "201", Type = RoomType.Double,  Floor = 2, Capacity = 2, BasePrice = 120m, Status = RoomStatus.Available },
            new Room { Number = "301", Type = RoomType.Suite,   Floor = 3, Capacity = 2, BasePrice = 250m, Status = RoomStatus.Occupied  },
            new Room { Number = "401", Type = RoomType.Family,  Floor = 4, Capacity = 4, BasePrice = 180m, Status = RoomStatus.Cleaning  },
            new Room { Number = "501", Type = RoomType.Penthouse, Floor = 5, Capacity = 4, BasePrice = 500m, Status = RoomStatus.Blocked }
        );

        _context.Bookings.AddRange(
            new Booking
            {
                Code = "BK-001", GuestId = 1, RoomId = 1,
                CheckInDate  = DateOnly.FromDateTime(DateTime.Today),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3)),
                PricePerNight = 80m, TotalPrice = 240m,
                Status = BookingStatus.Confirmed,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(3)
            },
            new Booking
            {
                Code = "BK-002", GuestId = 2, RoomId = 3,
                CheckInDate  = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                PricePerNight = 250m, TotalPrice = 750m,
                Status = BookingStatus.CheckedIn,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(2)
            },
            new Booking
            {
                Code = "BK-003", GuestId = 3, RoomId = 2,
                CheckInDate  = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7)),
                PricePerNight = 120m, TotalPrice = 240m,
                Status = BookingStatus.PendingPayment,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(1)
            }
        );

        _context.SaveChanges();
    }

    // ─────────────────────────────────────────────
    // Task 26: La aplicación inicia sin PostgreSQL
    // ─────────────────────────────────────────────
    [Fact]
    public async Task Database_CanConnect_WithoutPostgreSQL()
    {
        var canConnect = await _context.Database.CanConnectAsync();
        Assert.True(canConnect);
    }

    // ─────────────────────────────────────────────
    // Task 27: Validar habitaciones con InMemory
    // ─────────────────────────────────────────────
    [Fact]
    public async Task Rooms_SeedData_ShouldContainFiveRooms()
    {
        var count = await _context.Rooms.CountAsync();
        Assert.Equal(5, count);
    }

    [Fact]
    public async Task Rooms_Available_ShouldReturnOnlyAvailableRooms()
    {
        var available = await _context.Rooms
            .Where(r => r.Status == RoomStatus.Available)
            .ToListAsync();

        Assert.Equal(2, available.Count);
        Assert.All(available, r => Assert.Equal(RoomStatus.Available, r.Status));
    }

    [Fact]
    public async Task Rooms_CanFind_ByRoomNumber()
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Number == "301");

        Assert.NotNull(room);
        Assert.Equal(RoomType.Suite, room.Type);
        Assert.Equal(RoomStatus.Occupied, room.Status);
    }

    [Fact]
    public async Task Rooms_CanFilter_ByType()
    {
        var suites = await _context.Rooms
            .Where(r => r.Type == RoomType.Suite || r.Type == RoomType.Penthouse)
            .ToListAsync();

        Assert.Equal(2, suites.Count);
    }

    [Fact]
    public async Task Rooms_BasePrice_ShouldBePositive()
    {
        var rooms = await _context.Rooms.ToListAsync();
        Assert.All(rooms, r => Assert.True(r.BasePrice > 0));
    }

    // ─────────────────────────────────────────────
    // Task 27: Validar reservas con InMemory
    // ─────────────────────────────────────────────
    [Fact]
    public async Task Bookings_SeedData_ShouldContainThreeBookings()
    {
        var count = await _context.Bookings.CountAsync();
        Assert.Equal(3, count);
    }

    [Fact]
    public async Task Bookings_CanFind_ByCode()
    {
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Code == "BK-001");

        Assert.NotNull(booking);
        Assert.Equal(BookingStatus.Confirmed, booking.Status);
        Assert.Equal(240m, booking.TotalPrice);
    }

    [Fact]
    public async Task Bookings_ActiveToday_ShouldReturnCheckedInAndConfirmed()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var active = await _context.Bookings
            .Where(b => b.CheckInDate <= today && b.CheckOutDate >= today
                        && (b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.CheckedIn))
            .ToListAsync();

        Assert.Equal(2, active.Count);
    }

    [Fact]
    public async Task Bookings_PendingPayment_ShouldExist()
    {
        var pending = await _context.Bookings
            .Where(b => b.Status == BookingStatus.PendingPayment)
            .ToListAsync();

        Assert.Single(pending);
        Assert.Equal("BK-003", pending[0].Code);
    }

    [Fact]
    public async Task Bookings_TotalRevenue_ShouldSumCorrectly()
    {
        var total = await _context.Bookings.SumAsync(b => b.TotalPrice);
        Assert.Equal(1230m, total); // 240 + 750 + 240
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
