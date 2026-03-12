namespace Alti.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public class RoomNotAvailableException : DomainException
{
    public RoomNotAvailableException(int roomId, string currentStatus)
        : base($"Room {roomId} is not available. Current status: {currentStatus}.") { }
}

public class DoubleBookingException : DomainException
{
    public DoubleBookingException(int roomId, DateOnly checkIn, DateOnly checkOut)
        : base($"Room {roomId} already has a booking between {checkIn} and {checkOut}.") { }
}

public class BookingExpiredException : DomainException
{
    public BookingExpiredException(int bookingId)
        : base($"Booking {bookingId} has expired.") { }
}

public class InvalidBookingStatusException : DomainException
{
    public InvalidBookingStatusException(int bookingId, string currentStatus, string requiredStatus)
        : base($"Booking {bookingId} has invalid status. Current: {currentStatus}, Required: {requiredStatus}.") { }
}

public class PaymentNotApprovedException : DomainException
{
    public PaymentNotApprovedException(int bookingId)
        : base($"Booking {bookingId} does not have an approved payment.") { }
}

public class SeasonOverlapException : DomainException
{
    public SeasonOverlapException(DateOnly startDate, DateOnly endDate, string conflictingSeason)
        : base($"Season from {startDate} to {endDate} overlaps with {conflictingSeason}.") { }
}

public class DuplicateEmailException : DomainException
{
    public DuplicateEmailException(string email)
        : base($"Email '{email}' is already registered.") { }
}

public class InactiveUserException : DomainException
{
    public InactiveUserException(int userId)
        : base($"User {userId} is inactive.") { }
}

public class AccessDeniedException : DomainException
{
    public AccessDeniedException(int userId, string requiredRole)
        : base($"User {userId} does not have the required role: {requiredRole}.") { }
}