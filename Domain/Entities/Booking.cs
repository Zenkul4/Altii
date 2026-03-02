using Alti.Domain.Enums;
using Alti.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.domain.entities
{
    public class Booking : BaseEntity
    {
        private Booking() { }

        public string Code { get; private set; } = string.Empty;
        public int GuestId { get; private set; }
        public int RoomId { get; private set; }
        public int? AttendedById { get; private set; }
        public DateOnly CheckInDate { get; private set; }
        public DateOnly CheckOutDate { get; private set; }
        public decimal PricePerNight { get; private set; }
        public decimal TotalPrice { get; private set; }
        public BookingStatus Status { get; private set; } = BookingStatus.PendingPayment;
        public DateTimeOffset ExpiresAt { get; private set; }
        public string? Notes { get; private set; }


    }
}
