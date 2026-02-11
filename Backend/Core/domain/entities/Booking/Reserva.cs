using ALTI.Domain.Base;
using ALTI.Domain.Entities.Hotel;
using ALTI.Domain.Entities.Security;
using System;
using System.Collections.Generic;

namespace ALTI.Domain.Entities.Booking
{
    public class Reserva : AuditEntity
    {
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int CantidadHuespedes { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; }

        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public int HabitacionId { get; set; }
        public virtual Habitacion Habitacion { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; }
    }
}