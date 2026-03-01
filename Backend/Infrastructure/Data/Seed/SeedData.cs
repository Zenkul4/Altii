using Alti.Core.Domain.Entities.Hotel;
using Alti.Core.Domain.Entities.Security;
using Alti.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Alti.Infrastructure.Data.Seed;

public static class SeedData
{
    public static void AplicarSeed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>().HasData(
            Rol.CrearParaSeed(1, "Administrador", "Acceso total al sistema"),
            Rol.CrearParaSeed(2, "Recepcionista", "Gestión operativa del hotel"),
            Rol.CrearParaSeed(3, "Cliente", "Huésped del hotel")
        );

        modelBuilder.Entity<Categoria>().HasData(
            Categoria.CrearParaSeed(1, "Estándar", "Habitación básica", 2),
            Categoria.CrearParaSeed(2, "Suite", "Suite con vista al mar", 4),
            Categoria.CrearParaSeed(3, "Presidencial", "Máximo lujo", 6)
        );
    }
}