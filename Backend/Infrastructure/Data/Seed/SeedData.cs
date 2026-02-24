using Alti.Core.Domain.Entities.Hotel;
using Alti.Core.Domain.Entities.Security;
using Alti.Core.Domain.Enums;
using Alti.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alti.Infrastructure.Data.Seeds;

/// Datos semilla iniciales. Se ejecuta una sola vez al inicializar la base de datos.
public static class SeedData
{
    public static async Task InicializarAsync(HotelDbContext ctx)
    {
        await SeedCategorias(ctx);
        await SeedHabitaciones(ctx);
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedCategorias(HotelDbContext ctx)
    {
        if (ctx.Categorias.Any()) return;

        var categorias = new[]
        {
            Categoria.Crear("Estándar",   75.00m,  2, "Habitación básica con todas las amenidades.", multiplierTemporadaAlta: 1.3m),
            Categoria.Crear("Superior",   120.00m, 2, "Habitación superior con vista parcial al mar.", multiplierTemporadaAlta: 1.4m),
            Categoria.Crear("Deluxe",     180.00m, 3, "Habitación de lujo con balcón y vista al mar.", multiplierTemporadaAlta: 1.5m),
            Categoria.Crear("Suite",      300.00m, 4, "Suite ejecutiva con sala de estar separada.", multiplierTemporadaAlta: 1.6m),
            Categoria.Crear("Suite Pent", 600.00m, 6, "Penthouse con piscina privada y terraza.", multiplierTemporadaAlta: 2.0m),
        };

        await ctx.Categorias.AddRangeAsync(categorias);
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedHabitaciones(HotelDbContext ctx)
    {
        if (ctx.Habitaciones.Any()) return;


        var ids = ctx.Categorias.ToDictionary(c => c.Nombre, c => c.Id);

        var habitaciones = new List<Habitacion>();


        for (int i = 1; i <= 10; i++)
            habitaciones.Add(Habitacion.Crear($"1{i:D2}", 1, ids["Estándar"]));


        for (int i = 1; i <= 6; i++)
            habitaciones.Add(Habitacion.Crear($"2{i:D2}", 2, ids["Superior"]));

        
        for (int i = 1; i <= 4; i++)
            habitaciones.Add(Habitacion.Crear($"3{i:D2}", 3, ids["Deluxe"]));

        habitaciones.Add(Habitacion.Crear("401", 4, ids["Suite"]));
        habitaciones.Add(Habitacion.Crear("402", 4, ids["Suite"]));

        habitaciones.Add(Habitacion.Crear("PH1", 10, ids["Suite Pent"], "Penthouse principal"));

        await ctx.Habitaciones.AddRangeAsync(habitaciones);
    }
}