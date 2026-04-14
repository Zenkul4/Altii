using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models.Season;
using Desktop.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Desktop.ViewModels;

public partial class SeasonsViewModel : BaseViewModel
{
    private readonly ISeasonService _seasonService;
    private readonly IAuthService _authService;

    [ObservableProperty] private ObservableCollection<SeasonDto> _seasons = [];
    [ObservableProperty] private SeasonDto? _selected;
    [ObservableProperty] private bool _showDetail;
    [ObservableProperty] private bool _showCreateForm;
    [ObservableProperty] private bool _isEditing;

    [ObservableProperty] private string _formName = string.Empty;
    [ObservableProperty] private DateTime _formStartDate = DateTime.Today;
    [ObservableProperty] private DateTime _formEndDate = DateTime.Today.AddMonths(1);
    [ObservableProperty] private string _formMultiplier = "1.00";
    [ObservableProperty] private string _formDescription = string.Empty;

    public bool HasSelected => Selected is not null;

    public SeasonsViewModel(ISeasonService seasonService, IAuthService authService)
    {
        _seasonService = seasonService;
        _authService = authService;
    }

    partial void OnSelectedChanged(SeasonDto? value)
    {
        OnPropertyChanged(nameof(HasSelected));
        ShowDetail = value is not null;
        ShowCreateForm = false;
        IsEditing = false;
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

    [RelayCommand]
    private void ShowCreate()
    {
        Selected = null;
        ShowDetail = false;
        ShowCreateForm = true;
        IsEditing = false;
        FormName = string.Empty;
        FormStartDate = DateTime.Today;
        FormEndDate = DateTime.Today.AddMonths(1);
        FormMultiplier = "1.00";
        FormDescription = string.Empty;
    }

    [RelayCommand]
    private void ShowEdit()
    {
        if (Selected is null) return;
        ShowDetail = false;
        ShowCreateForm = true;
        IsEditing = true;
        FormName = Selected.Name;
        FormStartDate = DateTime.TryParse(Selected.StartDate, out var s) ? s : DateTime.Today;
        FormEndDate = DateTime.TryParse(Selected.EndDate, out var e) ? e : DateTime.Today.AddMonths(1);
        FormMultiplier = Selected.Multiplier.ToString(CultureInfo.InvariantCulture);
        FormDescription = Selected.Description ?? string.Empty;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(FormName)) { SetError("El nombre es obligatorio."); return; }
        if (_authService.CurrentUser is null) { SetError("No hay sesión activa."); return; }

        if (!decimal.TryParse(FormMultiplier.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var multiplier) || multiplier <= 0)
        {
            SetError("El multiplicador debe ser un número mayor a 0 (use . o , como separador decimal).");
            return;
        }

        IsLoading = true;
        ClearMessages();
        try
        {
            var dto = new CreateSeasonDto
            {
                Name = FormName.Trim(),
                StartDate = FormStartDate.ToString("yyyy-MM-dd"),
                EndDate = FormEndDate.ToString("yyyy-MM-dd"),
                Multiplier = multiplier,
                Description = string.IsNullOrWhiteSpace(FormDescription) ? null : FormDescription.Trim()
            };

            if (IsEditing && Selected is not null)
            {
                await _seasonService.UpdateAsync(Selected.Id, dto);
                SetSuccess("Temporada actualizada correctamente.");
            }
            else
            {
                await _seasonService.CreateAsync(dto, _authService.CurrentUser.Id);
                SetSuccess("Temporada creada correctamente.");
            }

            ShowCreateForm = false;
            IsEditing = false;
            Selected = null;
            await LoadAsync();
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