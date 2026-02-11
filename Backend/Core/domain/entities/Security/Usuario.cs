using ALTI.Domain.Base;
using ALTI.Domain.Entities.Booking;
using System.Collections.Generic;

namespace ALTI.Domain.Entities.Security
{
    public class Usuario : AuditEntity
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Telefono { get; set; }

        public int RolId { get; set; }
        public virtual Rol Rol { get; set; }
        public virtual ICollection<Reserva> Reservas { get; set; }
    }
}