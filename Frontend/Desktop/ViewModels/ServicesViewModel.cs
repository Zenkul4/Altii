using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models.AdditionalService;
using Desktop.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Desktop.ViewModels;

public partial class ServicesViewModel : BaseViewModel
{
    private readonly IAdditionalServiceService _service;

    [ObservableProperty] private ObservableCollection<AdditionalServiceDto> _services = [];
    [ObservableProperty] private AdditionalServiceDto? _selected;
    [ObservableProperty] private bool _showDetail;
    [ObservableProperty] private bool _showCreateForm;
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string _filterStatus = "Todos";

    // Form
    [ObservableProperty] private string _formName = string.Empty;
    [ObservableProperty] private string _formPrice = "0";
    [ObservableProperty] private string _formDescription = string.Empty;

    private List<AdditionalServiceDto> _allServices = [];

    public bool HasSelected => Selected is not null;
    public bool CanActivate => Selected is not null && !Selected.IsActive;
    public bool CanDeactivate => Selected is not null && Selected.IsActive;

    public List<string> StatusFilters => ["Todos", "Activos", "Inactivos"];

    public ServicesViewModel(IAdditionalServiceService service)
    {
        _service = service;
    }

    partial void OnSelectedChanged(AdditionalServiceDto? value)
    {
        OnPropertyChanged(nameof(HasSelected));
        OnPropertyChanged(nameof(CanActivate));
        OnPropertyChanged(nameof(CanDeactivate));
        ShowDetail = value is not null;
        ShowCreateForm = false;
        IsEditing = false;
    }

    partial void OnFilterStatusChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        var filtered = FilterStatus switch
        {
            "Activos" => _allServices.Where(s => s.IsActive),
            "Inactivos" => _allServices.Where(s => !s.IsActive),
            _ => _allServices.AsEnumerable()
        };
        Services = new ObservableCollection<AdditionalServiceDto>(filtered);
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsLoading = true;
        ClearMessages();
        try
        {
            _allServices = await _service.GetAllAsync();
            ApplyFilter();
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
        FormPrice = "0";
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
        FormPrice = Selected.Price.ToString(CultureInfo.InvariantCulture);
        FormDescription = Selected.Description ?? string.Empty;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(FormName)) { SetError("El nombre es obligatorio."); return; }

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
                await _service.UpdateAsync(Selected.Id, price,
                    string.IsNullOrWhiteSpace(FormDescription) ? null : FormDescription.Trim());
                SetSuccess("Servicio actualizado correctamente.");
            }
            else
            {
                var dto = new CreateAdditionalServiceDto
                {
                    Name = FormName.Trim(),
                    Price = price,
                    Description = string.IsNullOrWhiteSpace(FormDescription) ? null : FormDescription.Trim()
                };
                await _service.CreateAsync(dto);
                SetSuccess("Servicio creado correctamente.");
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
    private async Task ActivateAsync()
    {
        if (Selected is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            await _service.ActivateAsync(Selected.Id);
            SetSuccess("Servicio activado.");
            Selected = null;
            await LoadAsync();
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task DeactivateAsync()
    {
        if (Selected is null) return;
        IsLoading = true;
        ClearMessages();
        try
        {
            await _service.DeactivateAsync(Selected.Id);
            SetSuccess("Servicio desactivado.");
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