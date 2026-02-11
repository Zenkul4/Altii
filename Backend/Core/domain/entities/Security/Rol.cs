using ALTI.Domain.Base;
using System.Collections.Generic;

namespace ALTI.Domain.Entities.Security
{
    public class Rol : AuditEntity
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}