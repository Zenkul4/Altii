using Alti.Domain.Entities;

namespace Alti.Domain.Services.Interfaces;

public interface IBookingDomainService
{
    void Confirm(Booking booking, Payment payment);
    void RegisterCheckIn(Booking booking, int receptionistId);
    void RegisterCheckOut(Booking booking);
    void Cancel(Booking booking);
    void Expire(Booking booking);
}