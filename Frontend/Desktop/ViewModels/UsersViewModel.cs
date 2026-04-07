using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models.User;
using Desktop.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace Desktop.ViewModels;

public partial class UsersViewModel : BaseViewModel
{
    private readonly IUserService _userService;

    [ObservableProperty] private ObservableCollection<UserDto> _users = [];
    [ObservableProperty] private UserDto? _selected;
    [ObservableProperty] private bool _showDetail;
    [ObservableProperty] private bool _showCreateForm;
    [ObservableProperty] private string _filterRole = "All";

    // Form fields
    [ObservableProperty] private string _formFirstName = string.Empty;
    [ObservableProperty] private string _formLastName = string.Empty;
    [ObservableProperty] private string _formEmail = string.Empty;
    [ObservableProperty] private string _formPassword = string.Empty;
    [ObservableProperty] private string _formPhone = string.Empty;
    [ObservableProperty] private int _formRole = 0;

    public bool HasSelected => Selected is not null;
    public bool CanDeactivate => Selected?.IsActive ?? false;
    public bool CanReactivate => !(Selected?.IsActive ?? true);

    public List<string> RoleFilters => ["All", "Huésped", "Recepcionista", "Administrador"];
    public List<string> RoleOptions => ["Huésped", "Recepcionista"];

    public UsersViewModel(IUserService userService)
    {
        _userService = userService;
    }

    partial void OnSelectedChanged(UserDto? value)
    {
        OnPropertyChanged(nameof(HasSelected));
        OnPropertyChanged(nameof(CanDeactivate));
        OnPropertyChanged(nameof(CanReactivate));
        ShowDetail = value is not null;
        ShowCreateForm = false;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsLoading = true;
        ClearMessages();
        try
        {
            var list = await _userService.GetAllAsync();
            Users = new ObservableCollection<UserDto>(
                FilterRole == "All"
                    ? list
                    : list.Where(u => u.RoleLabel == FilterRole));
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
        FormFirstName = string.Empty;
        FormLastName = string.Empty;
        FormEmail = string.Empty;
        FormPassword = string.Empty;
        FormPhone = string.Empty;
        FormRole = 0;
        ClearMessages();
    }

    [RelayCommand]
    private async Task CreateAsync()
    {
        if (string.IsNullOrWhiteSpace(FormFirstName) ||
            string.IsNullOrWhiteSpace(FormLastName) ||
            string.IsNullOrWhiteSpace(FormEmail) ||
            string.IsNullOrWhiteSpace(FormPassword))
        {
            SetError("Complete todos los campos obligatorios.");
            return;
        }
        IsLoading = true;
        ClearMessages();
        try
        {
            await _userService.CreateAsync(new CreateUserDto
            {
                FirstName = FormFirstName.Trim(),
                LastName = FormLastName.Trim(),
                Email = FormEmail.Trim().ToLower(),
                Password = FormPassword,
                Phone = string.IsNullOrWhiteSpace(FormPhone) ? null : FormPhone.Trim(),
                Role = FormRole switch
                {
                    0 => 0,
                    1 => 1,
                    _ => 0
                },
            });
            SetSuccess($"Usuario {FormFirstName} creado correctamente.");
            ShowCreateForm = false;
            await LoadAsync();
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task DeactivateAsync()
    {
        if (Selected is null) return;
        IsLoading = true; ClearMessages();
        try { await _userService.DeactivateAsync(Selected.Id); SetSuccess("Usuario desactivado."); await LoadAsync(); Selected = null; }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task ReactivateAsync()
    {
        if (Selected is null) return;
        IsLoading = true; ClearMessages();
        try { await _userService.ReactivateAsync(Selected.Id); SetSuccess("Usuario reactivado."); await LoadAsync(); Selected = null; }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private void CloseDetail() { Selected = null; ShowDetail = false; }

    [RelayCommand]
    private void CloseCreateForm() { ShowCreateForm = false; }

    partial void OnFilterRoleChanged(string value) => _ = LoadAsync();
}