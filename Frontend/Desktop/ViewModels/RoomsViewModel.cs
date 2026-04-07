using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models.Room;
using Desktop.Services;
using Desktop.Services.Interfaces;
using System.Collections.ObjectModel;
using static Desktop.Models.Room.RoomDto;

namespace Desktop.ViewModels;

public partial class RoomsViewModel : BaseViewModel
{
    private readonly IRoomService _roomService;

    [ObservableProperty] private ObservableCollection<RoomDto> _rooms = [];
    [ObservableProperty] private RoomDto? _selected;
    [ObservableProperty] private bool _showDetail;
    [ObservableProperty] private bool _showCreateForm;
    [ObservableProperty] private string _filterStatus = "All";

    [ObservableProperty] private string _formNumber = string.Empty;
    [ObservableProperty] private string _formType = "Single";
    [ObservableProperty] private string _formFloor = string.Empty;
    [ObservableProperty] private string _formCapacity = string.Empty;
    [ObservableProperty] private string _formBasePrice = string.Empty;
    [ObservableProperty] private string _formDescription = string.Empty;

    public bool HasSelected => Selected is not null;
    public bool CanDisable => Selected?.Status == 0;
    public bool CanEnable => Selected?.Status == 4;
    public bool CanMarkAvail => Selected?.Status is 2 or 3;
    public bool CanMarkOccupied => Selected?.Status == 0;
    public bool CanMarkCleaning => Selected?.Status == 1;
    public bool CanMarkBlocked => Selected?.Status == 0;
    public bool CanReleaseBlock => Selected?.Status == 3;

    public List<string> StatusFilters => ["All", "Disponible", "Ocupada", "Limpieza", "Bloqueada", "Inactiva"];
    public List<string> RoomTypes => ["Single", "Double", "Suite", "Family", "Penthouse"];

    public RoomsViewModel(IRoomService roomService)
    {
        _roomService = roomService;
    }

    partial void OnSelectedChanged(RoomDto? value)
    {
        OnPropertyChanged(nameof(HasSelected));
        OnPropertyChanged(nameof(CanDisable));
        OnPropertyChanged(nameof(CanEnable));
        OnPropertyChanged(nameof(CanMarkAvail));
        OnPropertyChanged(nameof(CanMarkOccupied));
        OnPropertyChanged(nameof(CanMarkCleaning));
        OnPropertyChanged(nameof(CanMarkBlocked));
        OnPropertyChanged(nameof(CanReleaseBlock));
        ShowDetail = value is not null;
        ShowCreateForm = false;
    }
    [RelayCommand]
    private async Task MarkBlockedAsync()
    {
        if (Selected is null) return;
        IsLoading = true; ClearMessages();
        try
        {
            await _roomService.MarkBlockedAsync(Selected.Id);
            SetSuccess("Habitación bloqueada.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task ReleaseBlockAsync()
    {
        if (Selected is null) return;
        IsLoading = true; ClearMessages();
        try
        {
            await _roomService.ReleaseBlockAsync(Selected.Id);
            SetSuccess("Bloqueo liberado.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsLoading = true;
        ClearMessages();
        try
        {
            var list = await _roomService.GetAllAsync();
            Rooms = new ObservableCollection<RoomDto>(
                FilterStatus == "All"
                    ? list
                    : list.Where(r => r.StatusLabel == FilterStatus));
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private void ShowCreate()
    {
        Selected = null;
        ShowDetail = false;
        ShowCreateForm = true;
        FormNumber = string.Empty;
        FormType = "Single";
        FormFloor = string.Empty;
        FormCapacity = string.Empty;
        FormBasePrice = string.Empty;
        FormDescription = string.Empty;
        ClearMessages();
    }

    [RelayCommand]
    private async Task CreateAsync()
    {
        if (string.IsNullOrWhiteSpace(FormNumber) ||
            !short.TryParse(FormFloor, out var floor) ||
            !short.TryParse(FormCapacity, out var capacity) ||
            !decimal.TryParse(FormBasePrice, out var price))
        {
            SetError("Complete todos los campos correctamente.");
            return;
        }
        IsLoading = true;
        ClearMessages();
        try
        {
            await _roomService.CreateAsync(new CreateRoomDto
            {
                Number = FormNumber.Trim().ToUpper(),
                Type = FormType switch
                {
                    "Single" => 0,
                    "Double" => 1,
                    "Suite" => 2,
                    "Family" => 3,
                    "Penthouse" => 4,
                    _ => 0
                },
                Floor = floor,
                Capacity = capacity,
                BasePrice = price,
                Description = string.IsNullOrWhiteSpace(FormDescription) ? null : FormDescription.Trim(),
            });
            SetSuccess($"Habitación {FormNumber.ToUpper()} creada.");
            ShowCreateForm = false;
            await LoadAsync();
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task DisableAsync()
    {
        if (Selected is null) return;
        IsLoading = true; ClearMessages();
        try
        {
            await _roomService.DisableAsync(Selected.Id);
            SetSuccess("Habitación desactivada.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task EnableAsync()
    {
        if (Selected is null) return;
        IsLoading = true; ClearMessages();
        try
        {
            await _roomService.EnableAsync(Selected.Id);
            SetSuccess("Habitación activada.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task MarkAvailableAsync()
    {
        if (Selected is null) return;
        IsLoading = true; ClearMessages();
        try
        {
            await _roomService.MarkAvailableAsync(Selected.Id);
            SetSuccess("Habitación marcada como disponible.");
            await LoadAsync();
            Selected = null;
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task MarkCleaningAsync()
    {
        if (Selected is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            await _roomService.MarkCleaningAsync(Selected.Id);
            SetSuccess("Habitación en limpieza.");
            await LoadAsync();
            Selected = null;
        }
        catch (ApiException ex)
        {
            SetError($"Error {ex.StatusCode}: {ex.Message}");
        }
        catch (Exception ex)
        {
            SetError($"Error inesperado: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task MarkOccupiedAsync()
    {
        if (Selected is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            await _roomService.MarkOccupiedAsync(Selected.Id);
            SetSuccess("Habitación marcada como ocupada.");
            await LoadAsync();
            Selected = null;
        }
        catch (ApiException ex)
        {
            SetError($"Error {ex.StatusCode}: {ex.Message}");
        }
        catch (Exception ex)
        {
            SetError($"Error inesperado: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void CloseDetail() { Selected = null; ShowDetail = false; }

    [RelayCommand]
    private void CloseCreateForm() { ShowCreateForm = false; }

    partial void OnFilterStatusChanged(string value) => _ = LoadAsync();

}