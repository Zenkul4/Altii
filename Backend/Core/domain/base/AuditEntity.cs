using System;

namespace ALTI.Domain.Base
{
    public abstract class AuditEntity
    {
        public int Id { get; protected set; }

        // Auditoría de cambios registrados
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
        public DateTime? ActualizadoEn { get; set; }
        public int? CreadoPorId { get; set; }
        public int? ActualizadoPorId { get; set; }
        public bool EstaEliminado { get; private set; } = false;
        public DateTime? EliminadoEn { get; private set; }

        [System.ComponentModel.DataAnnotations.Timestamp]
        public byte[] RowVersion { get; set; } = [];

        public void EliminarLogico(int eliminadoPorId)
        {
            EstaEliminado = true;
            EliminadoEn = DateTime.UtcNow;
            ActualizadoPorId = eliminadoPorId;
            ActualizadoEn = DateTime.UtcNow;
        }

        public void MarcarActualizado(int actualizadoPorId)
        {
            ActualizadoPorId = actualizadoPorId;
            ActualizadoEn = DateTime.UtcNow;
        }
    }
}