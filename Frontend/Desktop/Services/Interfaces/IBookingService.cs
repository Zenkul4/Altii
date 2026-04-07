using Desktop.Models.Booking;

namespace Desktop.Services.Interfaces;

public interface IBookingService
{
    Task<List<BookingDto>> GetAllAsync(int page = 1, int pageSize = 50);   
    Task<List<BookingDto>> GetActiveAsync(int page = 1, int pageSize = 50);
    Task<BookingDto> GetByIdAsync(int id);
    Task<BookingDto> GetByCodeAsync(string code);
    Task ConfirmAsync(int id, int paymentId);
    Task CheckInAsync(int id, int receptionistId);
    Task CheckOutAsync(int id);
    Task CancelAsync(int id);
}