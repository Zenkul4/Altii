using Alti.Core.Domain.Base;
using Alti.Core.Domain.Entities.Booking;
using Alti.Core.Domain.Entities.Hotel;
using Alti.Core.Domain.Entities.Security;
using Alti.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alti.Infrastructure.Data;

/// DbContext principal
public sealed class HotelDbContext : DbContext
{
    private readonly int? _usuarioActualId;

    public HotelDbContext(DbContextOptions<HotelDbContext> options, ICurrentUserService currentUser)
        : base(options)
    {
        _usuarioActualId = currentUser.UsuarioId;
    }

    // --- DbSets ---
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
            if (typeof(EntidadBase).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(BuildSoftDeleteFilter(entityType.ClrType));
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Intercepta SaveChanges para: aplicar auditoría automática y manejar soft delete.
    /// El RowVersion (concurrencia) lo gestiona EF Core automáticamente.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        AplicarAuditoria();
        return await base.SaveChangesAsync(ct);
    }

    // --- Helpers privados ---

    private void AplicarAuditoria()
    {
        var ahora = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<EntidadBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreadoEn = ahora;
                    entry.Entity.CreadoPorId = _usuarioActualId;
                    break;

                case EntityState.Modified:
                    // Evitar que se sobreescriba CreadoEn en updates
                    entry.Property(e => e.CreadoEn).IsModified = false;
                    entry.Property(e => e.CreadoPorId).IsModified = false;
                    entry.Entity.MarcarActualizado(_usuarioActualId ?? 0);
                    break;

                case EntityState.Deleted:
                    // Convertir DELETE físico en soft delete lógico
                    entry.State = EntityState.Modified;
                    entry.Entity.EliminarLogico(_usuarioActualId ?? 0);
                    break;
            }
        }
    }

    private static System.Linq.Expressions.LambdaExpression BuildSoftDeleteFilter(Type tipo)
    {
        var param = System.Linq.Expressions.Expression.Parameter(tipo, "e");
        var prop = System.Linq.Expressions.Expression.Property(param, nameof(EntidadBase.EstaEliminado));
        var body = System.Linq.Expressions.Expression.Equal(prop,
                        System.Linq.Expressions.Expression.Constant(false));
        return System.Linq.Expressions.Expression.Lambda(body, param);
    }
}

/// <summary>
/// AGREGADO: Abstracción del usuario actual, permite obtener el ID del usuario
/// autenticado dentro del DbContext sin acoplarse a HttpContext.
/// </summary>
public interface ICurrentUserService
{
    int? UsuarioId { get; }
}