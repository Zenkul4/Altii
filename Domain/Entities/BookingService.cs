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
        public AdditionalService Service { get; private set; } = null!;
        public User RegisteredBy { get; private set; } = null!;

        public decimal Subtotal => Quantity * UnitPrice;

        public static BookingService Create(
            int bookingId,
            int serviceId,
            int registeredById,
            short quantity,
            decimal unitPrice)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            if (unitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));

            return new BookingService
            {
                BookingId = bookingId,
                ServiceId = serviceId,
                RegisteredById = registeredById,
                Quantity = quantity,
                UnitPrice = unitPrice
            };
        }
    }
}
