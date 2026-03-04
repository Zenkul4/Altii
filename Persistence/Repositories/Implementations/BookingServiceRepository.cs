using Alti.Domain.Entities;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class BookingServiceRepository : IBookingServiceRepository
{
    private readonly AppDbContext _context;

    public BookingServiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BookingService?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.BookingServices.FirstOrDefaultAsync(bs => bs.Id == id, ct);

    public async Task<IReadOnlyList<BookingService>> GetByBookingAsync(int bookingId, CancellationToken ct = default)
        => await _context.BookingServices.Where(bs => bs.BookingId == bookingId).ToListAsync(ct);

    public async Task AddAsync(BookingService bookingService, CancellationToken ct = default)
        => await _context.BookingServices.AddAsync(bookingService, ct);

    public void Update(BookingService bookingService)
        => _context.BookingServices.Update(bookingService);
}