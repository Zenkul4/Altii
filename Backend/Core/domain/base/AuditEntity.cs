using System;

namespace ALTI.Domain.Base
{
    public abstract class AuditEntity
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}