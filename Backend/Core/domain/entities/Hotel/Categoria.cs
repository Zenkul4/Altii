using Alti.Core.Domain.Base;
using System.Collections.Generic;

namespace Alti.Core.Domain.Entities.Hotel;

/// <summary>Categoría/tipo de habitación (Estándar, Suite, Deluxe, etc.).</summary>
public class Categoria : EntidadBase
{
    public string Nombre { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    public decimal PrecioBaseNoche { get; private set; }
    public int CapacidadMaxima { get; private set; }
    public string? ImagenUrl { get; private set; }

    // Tarifas dinámicas por temporada (AGREGADO: permite escalar pricing sin tocar entidad)
    public decimal? MultiplierTemporadaAlta { get; private set; }

    // Navegación
    public ICollection<Habitacion> Habitaciones { get; private set; } = [];

    private Categoria() { }

    public static Categoria Crear(string nombre, decimal precioBase, int capacidad,
                                  string? descripcion = null, string? imagenUrl = null,
                                  decimal? multiplierTemporadaAlta = null)
    {
        return new Categoria
        {
            Nombre = nombre.Trim(),
            PrecioBaseNoche = precioBase,
            CapacidadMaxima = capacidad,
            Descripcion = descripcion?.Trim(),
            ImagenUrl = imagenUrl,
            MultiplierTemporadaAlta = multiplierTemporadaAlta ?? 1.5m
        };
    }

    public void Actualizar(string nombre, decimal precioBase, int capacidad,
                           string? descripcion, string? imagenUrl, decimal? multiplier)
    {
        Nombre = nombre.Trim();
        PrecioBaseNoche = precioBase;
        CapacidadMaxima = capacidad;
        Descripcion = descripcion?.Trim();
        ImagenUrl = imagenUrl;
        MultiplierTemporadaAlta = multiplier ?? MultiplierTemporadaAlta;
    }

    /// <summary>Calcula el precio por noche aplicando multiplicador de temporada.</summary>
    public decimal CalcularPrecioNoche(bool esTemporadaAlta) =>
        esTemporadaAlta ? PrecioBaseNoche * (MultiplierTemporadaAlta ?? 1m) : PrecioBaseNoche;
}