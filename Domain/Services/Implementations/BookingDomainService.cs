using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Exceptions;
using Alti.Domain.Services.Interfaces;

namespace Alti.Domain.Services.Implementations;

public class BookingDomainService : IBookingDomainService
{
    public void Confirm(Booking booking, Payment payment)
    {
        if (booking.Status != BookingStatus.PendingPayment)
            throw new InvalidBookingStatusException(booking.Id, booking.Status.ToString(), nameof(BookingStatus.PendingPayment));

        if (DateTimeOffset.UtcNow >= booking.ExpiresAt)
            throw new BookingExpiredException(booking.Id);

        if (payment.Status != PaymentStatus.Approved)
            throw new PaymentNotApprovedException(booking.Id);

        booking.Status = BookingStatus.Confirmed;
        booking.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RegisterCheckIn(Booking booking, int receptionistId)
    {
        if (booking.Status != BookingStatus.Confirmed)
            throw new InvalidBookingStatusException(booking.Id, booking.Status.ToString(), nameof(BookingStatus.Confirmed));

        booking.AttendedById = receptionistId;
        booking.Status = BookingStatus.CheckedIn;
        booking.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RegisterCheckOut(Booking booking)
    {
        if (booking.Status != BookingStatus.CheckedIn)
            throw new InvalidBookingStatusException(booking.Id, booking.Status.ToString(), nameof(BookingStatus.CheckedIn));

        booking.Status = BookingStatus.CheckedOut;
        booking.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Cancel(Booking booking)
    {
        if (booking.Status is not BookingStatus.PendingPayment and not BookingStatus.Confirmed)
            throw new InvalidBookingStatusException(booking.Id, booking.Status.ToString(), "PendingPayment or Confirmed");

        booking.Status = BookingStatus.Cancelled;
        booking.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Expire(Booking booking)
    {
        if (booking.Status != BookingStatus.PendingPayment)
            throw new InvalidBookingStatusException(booking.Id, booking.Status.ToString(), nameof(BookingStatus.PendingPayment));

        booking.Status = BookingStatus.Expired;
        booking.UpdatedAt = DateTimeOffset.UtcNow;
    }

}