using CommunityToolkit.Mvvm.ComponentModel;

namespace Desktop.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _successMessage = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    partial void OnIsLoadingChanged(bool value) => OnLoadingStateChanged(value);

    protected virtual void OnLoadingStateChanged(bool isLoading) { }

    protected void SetError(string message)
    {
        ErrorMessage = message;
        SuccessMessage = string.Empty;
    }

    protected void SetSuccess(string message)
    {
        SuccessMessage = message;
        ErrorMessage = string.Empty;
    }

    protected void ClearMessages()
    {
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;
    }
}