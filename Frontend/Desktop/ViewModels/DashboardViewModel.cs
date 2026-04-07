using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models.Booking;
using Desktop.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Desktop.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly IBookingService _bookingService;
    private readonly IRoomService _roomService;
    private readonly IUserService _userService;

    [ObservableProperty] private int _totalBookings;
    [ObservableProperty] private int _confirmedBookings;
    [ObservableProperty] private int _checkedInBookings;
    [ObservableProperty] private int _pendingPayments;
    [ObservableProperty] private int _availableRooms;
    [ObservableProperty] private int _totalUsers;
    [ObservableProperty] private ObservableCollection<BookingDto> _recentBookings = [];

    public DashboardViewModel(
        IBookingService bookingService,
        IRoomService roomService,
        IUserService userService)
    {
        _bookingService = bookingService;
        _roomService = roomService;
        _userService = userService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsLoading = true;
        ClearMessages();
        try
        {
            var bookings = await _bookingService.GetAllAsync(1, 100);
            var rooms = await _roomService.GetAllAsync();
            var users = await _userService.GetAllAsync();

            TotalBookings = bookings.Count(b => b.Status == 0 || b.Status == 1 || b.Status == 2);
            ConfirmedBookings = bookings.Count(b => b.Status == 1);
            CheckedInBookings = bookings.Count(b => b.Status == 2);
            PendingPayments = bookings.Count(b => b.Status == 0);
            AvailableRooms = rooms.Count(r => r.Status == 0);
            TotalUsers = users.Count;
            RecentBookings = new ObservableCollection<BookingDto>(
                bookings.OrderByDescending(b => b.CreatedAt).Take(8));
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }
}