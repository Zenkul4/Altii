using ALTI.Domain.Base;
using System;

namespace ALTI.Domain.Entities.Booking
{
    public class Pago : AuditEntity
    {
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string MetodoPago { get; set; }
        public string ReferenciaExterna { get; set; }

        public int ReservaId { get; set; }
        public virtual Reserva Reserva { get; set; }
    }
}