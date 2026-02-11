using ALTI.Domain.Base;
using System.Collections.Generic;

namespace ALTI.Domain.Entities.Hotel
{
    public class Categoria : AuditEntity
    {
        public string Nombre { get; set; }
        public decimal PrecioBase { get; set; }
        public int CapacidadPersonas { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Habitacion> Habitaciones { get; set; }
    }
}