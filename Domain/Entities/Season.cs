using Alti.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.domain.entities
{
    public class Season : BaseEntity
    {
        private Season() { }

        public string Name { get; private set; } = string.Empty;
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public decimal Multiplier { get; private set; } = 1.00m;
        public string? Description { get; private set; }
        public int CreatedById { get; private set; }

    }
}
