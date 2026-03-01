using Alti.Core.Domain.Entities.Booking;
using Alti.Core.Domain.Entities.Hotel;
using Alti.Core.Domain.Entities.Security;
using ALTI.Domain.Base; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Alti.Infrastructure.Data;

public sealed class HotelDbContext : DbContext
{
    private readonly int? _usuarioActualId;

    public HotelDbContext(DbContextOptions<HotelDbContext> options, ICurrentUserService currentUser)
        : base(options)
    {
        _usuarioActualId = currentUser.UsuarioId;
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Habitacion> Habitaciones => Set<Habitacion>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<Pago> Pagos => Set<Pago>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotelDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(AuditEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(BuildSoftDeleteFilter(entityType.ClrType));
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        AplicarAuditoria();
        return await base.SaveChangesAsync(ct);
    }

    private void AplicarAuditoria()
    {
        var ahora = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreadoEn = ahora;
                    entry.Entity.CreadoPorId = _usuarioActualId;
                    break;

                case EntityState.Modified:
                    entry.Property(e => e.CreadoEn).IsModified = false;
                    entry.Property(e => e.CreadoPorId).IsModified = false;
                    entry.Entity.MarcarActualizado(_usuarioActualId ?? 0);
                    break;

                case EntityState.Deleted:
                  
                    entry.State = EntityState.Modified;
                    entry.Entity.EliminarLogico(_usuarioActualId ?? 0);
                    break;
            }
        }
    }

    private static System.Linq.Expressions.LambdaExpression BuildSoftDeleteFilter(Type tipo)
    {
        var param = System.Linq.Expressions.Expression.Parameter(tipo, "e");
        var prop = System.Linq.Expressions.Expression.Property(param, nameof(AuditEntity.EstaEliminado));
        var body = System.Linq.Expressions.Expression.Equal(prop, System.Linq.Expressions.Expression.Constant(false));
        return System.Linq.Expressions.Expression.Lambda(body, param);
    }
}

public interface ICurrentUserService
{
    int? UsuarioId { get; }
}