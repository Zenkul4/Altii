using Alti.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.domain.entities
{
    public class BookingService
    {
        private BookingService() { }

        public int Id { get; private set; }
        public int BookingId { get; private set; }
        public int ServiceId { get; private set; }
        public int RegisteredById { get; private set; }
        public short Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public DateTimeOffset RegisteredAt { get; private set; } = DateTimeOffset.UtcNow;

        public Booking Booking { get; private set; } = null!;
        public User RegisteredBy { get; private set; } = null!;


    }
}
