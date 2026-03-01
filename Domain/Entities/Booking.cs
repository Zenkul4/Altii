using Alti.Core.Domain.Enums;
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

        public IReadOnlyCollection<Payment> Payments
            => _payments.AsReadOnly();

        public IReadOnlyCollection<BookingService> Services
            => _services.AsReadOnly();

        private readonly List<Payment> _payments = new();
        private readonly List<BookingService> _services = new();

        public User? Guest { get; private set; }
        public Room? Room { get; private set; }
        public User? AttendedBy { get; private set; }

        public int Nights => CheckOutDate.DayNumber - CheckInDate.DayNumber;
        public bool IsStillValid => DateTimeOffset.UtcNow < ExpiresAt;
        public bool IsActive => Status is BookingStatus.Confirmed or BookingStatus.CheckedIn;
        public bool CanBeCancelled => Status is BookingStatus.PendingPayment or BookingStatus.Confirmed;

        public static Booking Create(
            string code,
            int guestId,
            int roomId,
            DateOnly checkInDate,
            DateOnly checkOutDate,
            decimal pricePerNight,
            int? attendedById = null,
            string? notes = null,
            int expirationMinutes = 15)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Booking code cannot be empty.", nameof(code));

            if (checkOutDate <= checkInDate)
                throw new ArgumentException("Check-out date must be after check-in date.", nameof(checkOutDate));

            if (pricePerNight <= 0)
                throw new ArgumentException("Price per night must be greater than zero.", nameof(pricePerNight));

            var nights = checkOutDate.DayNumber - checkInDate.DayNumber;
            var totalPrice = Math.Round(pricePerNight * nights, 2);

            return new Booking
            {
                Code = code.Trim().ToUpper(),
                GuestId = guestId,
                RoomId = roomId,
                AttendedById = attendedById,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                PricePerNight = pricePerNight,
                TotalPrice = totalPrice,
                Notes = notes?.Trim(),
                Status = BookingStatus.PendingPayment,
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(expirationMinutes)
            };
        }

        public void Confirm(Payment payment)
        {
            if (Status != BookingStatus.PendingPayment)
                throw new InvalidBookingStatusException(Id, Status.ToString(), nameof(BookingStatus.PendingPayment));

            if (!IsStillValid)
                throw new BookingExpiredException(Id);

            if (payment.Status != PaymentStatus.Approved)
                throw new PaymentNotApprovedException(Id);

            Status = BookingStatus.Confirmed;
            RegisterUpdate();
        }

        public void RegisterCheckIn(int receptionistId)
        {
            if (Status != BookingStatus.Confirmed)
                throw new InvalidBookingStatusException(Id, Status.ToString(), nameof(BookingStatus.Confirmed));

            AttendedById = receptionistId;
            Status = BookingStatus.CheckedIn;
            RegisterUpdate();
        }

        public void RegisterCheckOut()
        {
            if (Status != BookingStatus.CheckedIn)
                throw new InvalidBookingStatusException(Id, Status.ToString(), nameof(BookingStatus.CheckedIn));

            Status = BookingStatus.CheckedOut;
            RegisterUpdate();
        }

        public void Cancel()
        {
            if (!CanBeCancelled)
                throw new InvalidBookingStatusException(Id, Status.ToString(), "PendingPayment or Confirmed");

            Status = BookingStatus.Cancelled;
            RegisterUpdate();
        }

        public void Expire()
        {
            if (Status != BookingStatus.PendingPayment)
                throw new InvalidBookingStatusException(Id, Status.ToString(), nameof(BookingStatus.PendingPayment));

            Status = BookingStatus.Expired;
            RegisterUpdate();
        }

        internal void AddPayment(Payment payment) => _payments.Add(payment);
        internal void AddService(BookingService service) => _services.Add(service);
    }
}
