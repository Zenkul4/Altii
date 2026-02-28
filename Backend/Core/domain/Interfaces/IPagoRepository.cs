using Alti.Core.Domain.Entities.Booking;
using Alti.Core.Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IPagoRepository : IRepository<Pago>
{
    Task<Pago?> ObtenerPorReferenciaAsync(string referenciaPago, CancellationToken ct = default);
    Task<Pago?> ObtenerPorReferenciaExternaAsync(string referenciaExterna, CancellationToken ct = default);

    Task<IReadOnlyList<Pago>> ObtenerPorReservaAsync(int reservaId, CancellationToken ct = default);
    Task<IReadOnlyList<Pago>> ObtenerPorEstadoAsync(EstadoPago estado, CancellationToken ct = default);
    Task<IReadOnlyList<Pago>> ObtenerPorMetodoAsync(MetodoPago metodo, CancellationToken ct = default);

    Task<bool> CompletarAsync(int pagoId, string? referenciaExterna, CancellationToken ct = default);
    Task<bool> FallarAsync(int pagoId, string motivo, CancellationToken ct = default);
    Task<bool> IniciarProcesamientoAsync(int pagoId, CancellationToken ct = default);
    Task<bool> ReembolsarAsync(int pagoId, int autorizadoPorId, string motivo, CancellationToken ct = default);
}
