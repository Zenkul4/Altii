using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models.Booking;
using Desktop.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Desktop.ViewModels;

public partial class BookingsViewModel : BaseViewModel
{
    private readonly IBookingService _bookingService;
    private readonly IPaymentService _paymentService;
    private readonly IAuthService _authService;

    [ObservableProperty] private ObservableCollection<BookingDto> _bookings = [];
    [ObservableProperty] private BookingDto? _selected;
    [ObservableProperty] private string _searchCode = string.Empty;
    [ObservableProperty] private string _filterStatus = "All";
    [ObservableProperty] private bool _showDetail;

    public bool HasSelected => Selected is not null;
    public bool CanConfirm => Selected?.CanConfirm ?? false;
    public bool CanCheckIn => Selected?.CanCheckIn ?? false;
    public bool CanCheckOut => Selected?.CanCheckOut ?? false;
    public bool CanCancel => Selected?.CanCancel ?? false;

    public bool CanCashPayment => Selected?.Status == 0;

    public List<string> StatusFilters =>
    [
        "All", "Pago pendiente", "Confirmada", "En hotel", "Completada", "Cancelada"
    ];

    private IEnumerable<BookingDto> ApplyFilter(IEnumerable<BookingDto> list)
        => FilterStatus == "All" ? list : list.Where(b => b.StatusLabel == FilterStatus);

    public BookingsViewModel(
        IBookingService bookingService,
        IPaymentService paymentService,
        IAuthService authService)
    {
        _bookingService = bookingService;
        _paymentService = paymentService;
        _authService = authService;
    }

    partial void OnSelectedChanged(BookingDto? value)
    {
        OnPropertyChanged(nameof(HasSelected));
        OnPropertyChanged(nameof(CanConfirm));
        OnPropertyChanged(nameof(CanCheckIn));
        OnPropertyChanged(nameof(CanCheckOut));
        OnPropertyChanged(nameof(CanCancel));
        OnPropertyChanged(nameof(CanCashPayment));
        ShowDetail = value is not null;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsLoading = true;
        ClearMessages();
        try
        {
            var list = await _bookingService.GetAllAsync(1, 100);
            Bookings = new ObservableCollection<BookingDto>(ApplyFilter(list));
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task SearchByCodeAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchCode)) { await LoadAsync(); return; }
        IsLoading = true;
        ClearMessages();
        try
        {
            var b = await _bookingService.GetByCodeAsync(SearchCode.Trim().ToUpper());
            Bookings = [b];
            Selected = b;
        }
        catch { SetError($"No se encontró la reserva {SearchCode}."); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        if (Selected is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            var payments = await _paymentService.GetByBookingAsync(Selected.Id);
            var pending = payments.FirstOrDefault(p => p.Status == 0)
           ?? payments.FirstOrDefault();
              payments.FirstOrDefault();

            if (pending is null) { SetError("No hay pagos registrados para esta reserva."); return; }

            await _paymentService.ApproveAsync(pending.Id, $"STAFF-{DateTime.Now:yyyyMMddHHmmss}");
            await _bookingService.ConfirmAsync(Selected.Id, pending.Id);
            SetSuccess("Reserva confirmada correctamente.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task CheckInAsync()
    {
        if (Selected is null || _authService.CurrentUser is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            await _bookingService.CheckInAsync(Selected.Id, _authService.CurrentUser.Id);
            SetSuccess("Check-in registrado correctamente.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task CheckOutAsync()
    {
        if (Selected is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            await _bookingService.CheckOutAsync(Selected.Id);
            SetSuccess("Check-out registrado correctamente.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        if (Selected is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            await _bookingService.CancelAsync(Selected.Id);
            SetSuccess("Reserva cancelada.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task CashPaymentAsync()
    {
        if (Selected is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            await _paymentService.RegisterCashPaymentAsync(Selected.Id);
            SetSuccess($"Pago en efectivo registrado y reserva {Selected.Code} confirmada.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private void CloseDetail() { Selected = null; ShowDetail = false; }

    partial void OnFilterStatusChanged(string value) => _ = LoadAsync();
}

