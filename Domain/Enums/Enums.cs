namespace Alti.Domain.Enums;

public enum UserRole
{
    Guest,
    Receptionist,
    Administrator
}

public enum RoomStatus
{
    Available,
    Occupied,
    Cleaning,
    Blocked,
    Inactive
}

public enum RoomType
{
    Single,
    Double,
    Suite,
    Family,
    Penthouse
}

public enum BookingStatus
{
    PendingPayment,
    Confirmed,
    CheckedIn,
    CheckedOut,
    Cancelled,
    Expired
}

public enum PaymentStatus
{
    Pending,
    Approved,
    Rejected,
    Refunded
}

public enum AuditAction
{
    Create,
    Update,
    Delete,
    Login,
    Logout,
    CheckIn,
    CheckOut,
    Payment,
    Cancellation,
    RoomBlock,
    RoomRelease
}

public enum AuditEntity
{
    User,
    Room,
    Booking,
    Payment,
    Rate,
    Season
}