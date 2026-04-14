using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;
using System.Text.Json;

namespace Persistence.Context;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is not AuditLog &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
            .ToList();

        if (!entries.Any()) return await base.SaveChangesAsync(cancellationToken);

        var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRoleString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

        int? userId = int.TryParse(userIdString, out var parsedId) ? parsedId : null;
        UserRole? executorRole = Enum.TryParse<UserRole>(userRoleString, out var parsedRole) ? parsedRole : null;
        var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        var auditEntries = new List<AuditLog>();

        foreach (var entry in entries)
        {
            var auditLog = new AuditLog
            {
                UserId = userId,
                ExecutorRole = executorRole,
                Action = entry.State switch
                {
                    EntityState.Added => AuditAction.Create,
                    EntityState.Modified => AuditAction.Update,
                    EntityState.Deleted => AuditAction.Delete,
                    _ => AuditAction.Update
                },
                Entity = Enum.TryParse<AuditEntity>(entry.Entity.GetType().Name, out var entityType) ? entityType : AuditEntity.User,
                EntityId = GetEntityId(entry),
                PreviousData = GetPreviousData(entry),
                NewData = GetNewData(entry),
                IpAddress = ipAddress,
                Description = $"Automated {entry.State} log",
                ExecutedAt = DateTimeOffset.UtcNow
            };
            auditEntries.Add(auditLog);
        }

        AuditLogs.AddRange(auditEntries);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private static int? GetEntityId(EntityEntry entry)
    {
        var property = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
        if (property?.CurrentValue != null && int.TryParse(property.CurrentValue.ToString(), out var id))
        {
            return id;
        }
        return null;
    }

    private static string? GetPreviousData(EntityEntry entry)
    {
        if (entry.State == EntityState.Added) return null;
        var details = new Dictionary<string, object?>();
        foreach (var prop in entry.Properties)
        {
            if (prop.IsTemporary || prop.Metadata.IsPrimaryKey()) continue;
            if (entry.State == EntityState.Deleted || (entry.State == EntityState.Modified && prop.IsModified))
            {
                details[prop.Metadata.Name] = prop.OriginalValue;
            }
        }
        return details.Any() ? JsonSerializer.Serialize(details) : null;
    }

    private static string? GetNewData(EntityEntry entry)
    {
        if (entry.State == EntityState.Deleted) return null;
        var details = new Dictionary<string, object?>();
        foreach (var prop in entry.Properties)
        {
            if (prop.IsTemporary || prop.Metadata.IsPrimaryKey()) continue;
            if (entry.State == EntityState.Added || (entry.State == EntityState.Modified && prop.IsModified))
            {
                details[prop.Metadata.Name] = prop.CurrentValue;
            }
        }
        return details.Any() ? JsonSerializer.Serialize(details) : null;
    }
}