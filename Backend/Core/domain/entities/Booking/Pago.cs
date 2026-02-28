using Alti.Core.Domain.Common;
using Alti.Core.Domain.Enums;
using ALTI.Domain.Base;
using System;

namespace Alti.Core.Domain.Entities.Booking;


/// Entidad Pago. Registra cada transacción de pago asociada a una Reserva.

public class Pago : AuditEntity
{
    public string ReferenciaPago { get; private set; } = string.Empty;
    public decimal Monto { get; private set; }
    public MetodoPago MetodoPago { get; private set; }
    public EstadoPago Estado { get; private set; } = EstadoPago.Pendiente;
    public DateTime? FechaProcesado { get; private set; }
    public string? ReferenciaExterna { get; private set; } 
    public string? MotivoFallo { get; private set; }
 
    public DateTime? FechaReembolso { get; private set; }
    public string? MotivoReembolso { get; private set; }
    public int? AutorizadoPorId { get; private set; }

    public int ReservaId { get; private set; }
    public Reserva Reserva { get; private set; } = null!;

    private Pago() { }

    public static Pago Crear(int reservaId, decimal monto, MetodoPago metodo,
                             string? referenciaExterna = null)
    {
        return new Pago
        {
            ReferenciaPago = GenerarReferencia(),
            ReservaId = reservaId,
            Monto = monto,
            MetodoPago = metodo,
            ReferenciaExterna = referenciaExterna?.Trim()
        };
    }

    public void Completar(string? referenciaExterna = null)
    {
        Estado = EstadoPago.Completado;
        FechaProcesado = DateTime.UtcNow;
        ReferenciaExterna = referenciaExterna ?? ReferenciaExterna;
    }

    public void Fallar(string motivo)
    {
        Estado = EstadoPago.Fallido;
        MotivoFallo = motivo.Trim();
    }

    public void IniciarProcesamiento() => Estado = EstadoPago.Procesando;

    public Result Reembolsar(int autorizadoPorId, string motivo)
    {
        if (Estado != EstadoPago.Completado)
            return Result.Fallo("Solo se pueden reembolsar pagos completados.");
        if (string.IsNullOrWhiteSpace(motivo))
            return Result.Fallo("Debe especificarse el motivo del reembolso.");

        Estado = EstadoPago.Reembolsado;
        FechaReembolso = DateTime.UtcNow;
        MotivoReembolso = motivo.Trim();
        AutorizadoPorId = autorizadoPorId;
        return Result.Ok();
    }

    private static string GenerarReferencia() =>
        $"PAY-{Guid.NewGuid().ToString("N")[..12].ToUpper()}";
}