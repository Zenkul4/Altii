namespace Alti.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public class RoomNotAvailableException : DomainException
{
    public int RoomId { get; }
    public string CurrentStatus { get; }

    public RoomNotAvailableException(int roomId, string currentStatus)
        : base($"Room {roomId} is not available. Current status: {currentStatus}.")
    {
        RoomId = roomId;
        CurrentStatus = currentStatus;
    }
}

public class DoubleBookingException : DomainException
{
    public int RoomId { get; }
    public DateOnly CheckIn { get; }
    public DateOnly CheckOut { get; }

    public DoubleBookingException(int roomId, DateOnly checkIn, DateOnly checkOut)
        : base($"Room {roomId} already has an active booking between {checkIn:dd/MM/yyyy} and {checkOut:dd/MM/yyyy}.")
    {
        RoomId = roomId;
        CheckIn = checkIn;
        CheckOut = checkOut;
    }
}

public class BookingExpiredException : DomainException
{
    public int BookingId { get; }

    public BookingExpiredException(int bookingId)
        : base($"Booking {bookingId} has expired. The payment time limit was exceeded and the room has been released.")
    {
        BookingId = bookingId;
    }
}

public class InvalidBookingStatusException : DomainException
{
    public int BookingId { get; }
    public string CurrentStatus { get; }
    public string RequiredStatus { get; }

    public InvalidBookingStatusException(int bookingId, string currentStatus, string requiredStatus)
        : base($"Booking {bookingId} cannot perform this operation. Current status: '{currentStatus}'. Required: '{requiredStatus}'.")
    {
        BookingId = bookingId;
        CurrentStatus = currentStatus;
        RequiredStatus = requiredStatus;
    }
}

public class PaymentNotApprovedException : DomainException
{
    public int BookingId { get; }

    public PaymentNotApprovedException(int bookingId)
        : base($"Booking {bookingId} cannot be confirmed: no approved payment found.")
    {
        BookingId = bookingId;
    }
}

public class SeasonOverlapException : DomainException
{
    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }
    public string ConflictingSeason { get; }

    public SeasonOverlapException(DateOnly startDate, DateOnly endDate, string conflictingSeason)
        : base($"The range {startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy} overlaps with season '{conflictingSeason}'.")
    {
        StartDate = startDate;
        EndDate = endDate;
        ConflictingSeason = conflictingSeason;
    }
}

public class DuplicateEmailException : DomainException
{
    public string Email { get; }

    public DuplicateEmailException(string email)
        : base($"A user with email '{email}' already exists.")
    {
        Email = email;
    }
}

public class InactiveUserException : DomainException
{
    public int UserId { get; }

    public InactiveUserException(int userId)
        : base($"User {userId} is not active in the system.")
    {
        UserId = userId;
    }
}

public class AccessDeniedException : DomainException
{
    public int UserId { get; }
    public string RequiredRole { get; }

    public AccessDeniedException(int userId, string requiredRole)
        : base($"User {userId} does not have permission to perform this operation. Required role: '{requiredRole}'.")
    {
        UserId = userId;
        RequiredRole = requiredRole;
    }
}