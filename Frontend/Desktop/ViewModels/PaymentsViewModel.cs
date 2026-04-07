using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models.Payment;
using Desktop.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Desktop.ViewModels;

public partial class PaymentsViewModel : BaseViewModel
{
    private readonly IPaymentService _paymentService;
    private readonly IBookingService _bookingService;

    [ObservableProperty] private ObservableCollection<PaymentDto> _payments = [];
    [ObservableProperty] private PaymentDto? _selected;
    [ObservableProperty] private string _searchBookingId = string.Empty;
    [ObservableProperty] private bool _showDetail;
    [ObservableProperty] private string _externalReference = string.Empty;

    public bool HasSelected => Selected is not null;
    public bool CanApprove => Selected?.CanApprove ?? false;
    public bool CanReject => Selected?.CanReject ?? false;
    public bool CanRefund => Selected?.CanRefund ?? false;

    public PaymentsViewModel(IPaymentService paymentService, IBookingService bookingService)
    {
        _paymentService = paymentService;
        _bookingService = bookingService;
    }

    partial void OnSelectedChanged(PaymentDto? value)
    {
        OnPropertyChanged(nameof(HasSelected));
        OnPropertyChanged(nameof(CanApprove));
        OnPropertyChanged(nameof(CanReject));
        OnPropertyChanged(nameof(CanRefund));
        ShowDetail = value is not null;
        ExternalReference = string.Empty;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsLoading = true;
        ClearMessages();
        try
        {
            var bookings = await _bookingService.GetAllAsync(1, 100);
            var allPayments = new List<PaymentDto>();
            foreach (var b in bookings)
            {
                var pays = await _paymentService.GetByBookingAsync(b.Id);
                allPayments.AddRange(pays);
            }
            Payments = new ObservableCollection<PaymentDto>(
                allPayments.OrderByDescending(p => p.CreatedAt));
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task SearchByBookingAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchBookingId)) { await LoadAsync(); return; }
        if (!int.TryParse(SearchBookingId.Trim(), out var bookingId))
        {
            SetError("Ingrese un ID de reserva válido.");
            return;
        }
        IsLoading = true;
        ClearMessages();
        try
        {
            var pays = await _paymentService.GetByBookingAsync(bookingId);
            Payments = new ObservableCollection<PaymentDto>(pays);
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task ApproveAsync()
    {
        if (Selected is null) return;
        var ref_ = string.IsNullOrWhiteSpace(ExternalReference)
            ? $"STAFF-{DateTime.Now:yyyyMMddHHmmss}"
            : ExternalReference.Trim();

        IsLoading = true;
        ClearMessages();
        try
        {
            await _paymentService.ApproveAsync(Selected.Id, ref_);
            SetSuccess("Pago aprobado correctamente.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task RejectAsync()
    {
        if (Selected is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            await _paymentService.RejectAsync(Selected.Id);
            SetSuccess("Pago rechazado.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task RefundAsync()
    {
        if (Selected is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            await _paymentService.RefundAsync(Selected.Id);
            SetSuccess("Pago reembolsado.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private void CloseDetail() { Selected = null; ShowDetail = false; }
}