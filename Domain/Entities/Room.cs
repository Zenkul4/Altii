using Alti.Domain.Enums;
using Alti.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.domain.entities
{
    public class Room : BaseEntity
    {
        private Room() { }

        public string Number { get; private set; } = string.Empty;
        public RoomType Type { get; private set; }
        public short Floor { get; private set; }
        public short Capacity { get; private set; }
        public decimal BasePrice { get; private set; }
        public string? Description { get; private set; }
        public RoomStatus Status { get; private set; } = RoomStatus.Available;
        public int RowVersion { get; private set; } = 0;


    }
}
