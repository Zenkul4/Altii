using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Core.domain.entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;

namespace Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Rate> Rates { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<AdditionalService> AdditionalServices { get; set; }
    public DbSet<BookingService> BookingServices { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("alti");

        modelBuilder.HasPostgresExtension("btree_gist");

        modelBuilder.HasPostgresEnum<UserRole>();
        modelBuilder.HasPostgresEnum<RoomStatus>();
        modelBuilder.HasPostgresEnum<RoomType>();
        modelBuilder.HasPostgresEnum<BookingStatus>();
        modelBuilder.HasPostgresEnum<PaymentStatus>();
        modelBuilder.HasPostgresEnum<AuditAction>();
        modelBuilder.HasPostgresEnum<AuditEntity>();

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new SeasonConfiguration());
        modelBuilder.ApplyConfiguration(new RateConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new AdditionalServiceConfiguration());
        modelBuilder.ApplyConfiguration(new BookingServiceConfiguration());
        modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
    }
}