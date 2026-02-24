using Alti.Core.Domain.Base;
using Alti.Core.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Alti.Core.Domain.Entities.Hotel;

/// <summary>
/// Entidad Habitación. Cada habitación pertenece a una Categoría y mantiene
/// su estado de disponibilidad. Los cambios de estado se realizan mediante
/// métodos de dominio para garantizar invariantes.
/// </summary>
public class Habitacion : EntidadBase
{
    public string Numero { get; private set; } = string.Empty;
    public int Piso { get; private set; }
    public EstadoHabitacion Estado { get; private set; } = EstadoHabitacion.Disponible;
    public string? Notas { get; private set; }
    public bool Activa { get; private set; } = true;

    // AGREGADO: metadatos para reporting y housekeeping
    public DateTime? UltimaLimpieza { get; private set; }
    public int? LimpiadaPorEmpleadoId { get; private set; }

    // FK y navegación
    public int CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;

    public ICollection<Booking.Reserva> Reservas { get; private set; } = [];

    private Habitacion() { }

    public static Habitacion Crear(string numero, int piso, int categoriaId, string? notas = null)
    {
        return new Habitacion
        {
            Numero = numero.Trim().ToUpper(),
            Piso = piso,
            CategoriaId = categoriaId,
            Notas = notas?.Trim()
        };
    }

    // --- Transiciones de estado del dominio ---

    public void MarcarOcupada() => CambiarEstado(EstadoHabitacion.Ocupada);
    public void MarcarDisponible() => CambiarEstado(EstadoHabitacion.Disponible);
    public void EnviarALimpieza() => CambiarEstado(EstadoHabitacion.EnLimpieza);
    public void EnviarAMantenimiento() => CambiarEstado(EstadoHabitacion.Mantenimiento);
    public void DesactivarServicio() => CambiarEstado(EstadoHabitacion.FueraDeServicio);

    /// <summary>Registra cuando se completó la limpieza de la habitación.</summary>
    public void RegistrarLimpieza(int empleadoId)
    {
        UltimaLimpieza = DateTime.UtcNow;
        LimpiadaPorEmpleadoId = empleadoId;
        CambiarEstado(EstadoHabitacion.Disponible);
    }

    public bool EstaDisponible() => Estado == EstadoHabitacion.Disponible && Activa && !EstaEliminado;

    public void Desactivar() => Activa = false;
    public void Activar() => Activa = true;

    public void ActualizarNotas(string? notas) => Notas = notas?.Trim();

    private void CambiarEstado(EstadoHabitacion nuevoEstado) => Estado = nuevoEstado;
}