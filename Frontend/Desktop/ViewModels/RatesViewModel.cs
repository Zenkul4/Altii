using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models.Rate;
using Desktop.Models.Season;
using Desktop.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Desktop.ViewModels;

public partial class RatesViewModel : BaseViewModel
{
    private readonly IRateService _rateService;
    private readonly ISeasonService _seasonService;
    private readonly IAuthService _authService;

    [ObservableProperty] private ObservableCollection<SeasonDto> _seasons = [];
    [ObservableProperty] private SeasonDto? _selectedSeason;
    [ObservableProperty] private ObservableCollection<RateDto> _rates = [];
    [ObservableProperty] private RateDto? _selected;
    [ObservableProperty] private bool _showDetail;
    [ObservableProperty] private bool _showCreateForm;
    [ObservableProperty] private bool _isEditing;

    [ObservableProperty] private int _formRoomType;
    [ObservableProperty] private string _formPrice = "0";

    public bool HasSelected => Selected is not null;
    public bool HasSeason => SelectedSeason is not null;

    public List<string> RoomTypes => ["Single", "Double", "Suite", "Family", "Penthouse"];

    public RatesViewModel(IRateService rateService, ISeasonService seasonService, IAuthService authService)
    {
        _rateService = rateService;
        _seasonService = seasonService;
        _authService = authService;
    }

    partial void OnSelectedChanged(RateDto? value)
    {
        OnPropertyChanged(nameof(HasSelected));
        ShowDetail = value is not null;
        ShowCreateForm = false;
        IsEditing = false;
    }

    partial void OnSelectedSeasonChanged(SeasonDto? value)
    {
        OnPropertyChanged(nameof(HasSeason));
        if (value is not null) _ = LoadRatesAsync();
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsLoading = true;
        ClearMessages();
        try
        {
            var list = await _seasonService.GetAllAsync();
            Seasons = new ObservableCollection<SeasonDto>(list);
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    private async Task LoadRatesAsync()
    {
        if (SelectedSeason is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            var list = await _rateService.GetBySeasonAsync(SelectedSeason.Id);
            Rates = new ObservableCollection<RateDto>(list);
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private void ShowCreate()
    {
        if (SelectedSeason is null) { SetError("Seleccione una temporada primero."); return; }
        Selected = null;
        ShowDetail = false;
        ShowCreateForm = true;
        IsEditing = false;
        FormRoomType = 0;
        FormPrice = "0";
    }

    [RelayCommand]
    private void ShowEdit()
    {
        if (Selected is null) return;
        ShowDetail = false;
        ShowCreateForm = true;
        IsEditing = true;
        FormRoomType = Selected.RoomType;
        FormPrice = Selected.PricePerNight.ToString(CultureInfo.InvariantCulture);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!decimal.TryParse(FormPrice.Replace(',', '.'), NumberStyles.Any,
            CultureInfo.InvariantCulture, out var price) || price <= 0)
        {
            SetError("El precio debe ser un número mayor a 0.");
            return;
        }

        IsLoading = true;
        ClearMessages();
        try
        {
            if (IsEditing && Selected is not null)
            {
                await _rateService.UpdateAsync(Selected.Id, price);
                SetSuccess("Tarifa actualizada correctamente.");
            }
            else
            {
                if (SelectedSeason is null || _authService.CurrentUser is null) return;
                var dto = new CreateRateDto
                {
                    SeasonId = SelectedSeason.Id,
                    RoomType = FormRoomType,
                    PricePerNight = price
                };
                await _rateService.CreateAsync(dto, _authService.CurrentUser.Id);
                SetSuccess("Tarifa creada correctamente.");
            }

            ShowCreateForm = false;
            IsEditing = false;
            Selected = null;
            await LoadRatesAsync();
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private void CancelCreate()
    {
        ShowCreateForm = false;
        IsEditing = false;
    }

    [RelayCommand]
    private void CloseDetail() { Selected = null; ShowDetail = false; }
}