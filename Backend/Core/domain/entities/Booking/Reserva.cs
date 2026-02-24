using Alti.Core.Domain.Base;
using Alti.Core.Domain.Common;
using Alti.Core.Domain.Entities.Hotel;
using Alti.Core.Domain.Entities.Security;
using Alti.Core.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Alti.Core.Domain.Entities.Booking;

/// Entidad raíz del agregado Reserva. 
public class Reserva : EntidadBase
{
    public string CodigoConfirmacion { get; private set; } = string.Empty;
    public DateTime FechaEntrada { get; private set; }
    public DateTime FechaSalida { get; private set; }
    public int NumeroHuespedes { get; private set; }
    public EstadoReserva Estado { get; private set; } = EstadoReserva.Pendiente;
    public decimal TotalCalculado { get; private set; }
    public string? NotasEspeciales { get; private set; }
    public string? MotivosCancelacion { get; private set; }
    public DateTime? FechaCheckIn { get; private set; }
    public DateTime? FechaCheckOut { get; private set; }

    public bool PagoRequerido { get; private set; } = true;
    public bool PagoManualPendiente { get; private set; } = false;


    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    public int HabitacionId { get; private set; }
    public Habitacion Habitacion { get; private set; } = null!;

    public ICollection<Pago> Pagos { get; private set; } = [];

    private Reserva() { }

    public static Reserva Crear(int usuarioId, int habitacionId, DateTime entrada,
                                DateTime salida, int huespedes, decimal precioNoche,
                                string? notas = null, bool pagoRequerido = true)
    {
        var noches = (salida - entrada).Days;

        return new Reserva
        {
            CodigoConfirmacion = GenerarCodigo(),
            UsuarioId = usuarioId,
            HabitacionId = habitacionId,
            FechaEntrada = entrada.Date,
            FechaSalida = salida.Date,
            NumeroHuespedes = huespedes,
            TotalCalculado = precioNoche * noches,
            NotasEspeciales = notas?.Trim(),
            PagoRequerido = pagoRequerido,
            PagoManualPendiente = !pagoRequerido,
            Estado = EstadoReserva.Pendiente
        };
    }



    public Result Confirmar()
    {
        if (Estado != EstadoReserva.Pendiente)
            return Result.Fallo("Solo reservas Pendientes pueden confirmarse.");

        Estado = EstadoReserva.Confirmada;
        return Result.Ok();
    }

    public Result HacerCheckIn()
    {
        if (Estado != EstadoReserva.Confirmada)
            return Result.Fallo("La reserva debe estar Confirmada para hacer Check-In.");
        if (FechaEntrada.Date > DateTime.UtcNow.Date)
            return Result.Fallo("El Check-In no puede realizarse antes de la fecha de entrada.");

        Estado = EstadoReserva.CheckIn;
        FechaCheckIn = DateTime.UtcNow;
        return Result.Ok();
    }

    public Result HacerCheckOut()
    {
        if (Estado != EstadoReserva.CheckIn)
            return Result.Fallo("El huésped debe haber hecho Check-In primero.");

        Estado = EstadoReserva.CheckOut;
        FechaCheckOut = DateTime.UtcNow;
        return Result.Ok();
    }

    public Result Cancelar(string motivo)
    {
        if (Estado is EstadoReserva.CheckIn or EstadoReserva.CheckOut)
            return Result.Fallo("No se puede cancelar una reserva en proceso o completada.");
        if (string.IsNullOrWhiteSpace(motivo))
            return Result.Fallo("Debe especificarse el motivo de cancelación.");

        Estado = EstadoReserva.Cancelada;
        MotivosCancelacion = motivo.Trim();
        return Result.Ok();
    }

    public void ValidarPagoManual()
    {
        PagoManualPendiente = false;
        PagoRequerido = true;
    }

    public int TotalNoches() => (FechaSalida - FechaEntrada).Days;
    public bool EstaPagada() => Pagos.Any(p => p.Estado == EstadoPago.Completado);
    public decimal TotalPagado() => Pagos.Where(p => p.Estado == EstadoPago.Completado)
                                            .Sum(p => p.Monto);

    private static string GenerarCodigo() =>
        $"ALT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
}