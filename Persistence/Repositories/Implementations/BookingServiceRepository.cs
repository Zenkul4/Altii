using Alti.Domain.Entities;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class BookingServiceRepository : BaseRepository<BookingService>, IBookingServiceRepository
{
    public BookingServiceRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<BookingService>> GetByBookingAsync(int bookingId, CancellationToken ct = default)
        => await DbSet.Where(bs => bs.BookingId == bookingId).ToListAsync(ct);
}