using System.Windows;
using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop.Views.Pages;

public partial class UsersPage : UserControl
{
    public UsersPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is UsersViewModel vm)
            _ = vm.LoadAsync();
    }

    private void PwdBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is UsersViewModel vm)
            vm.FormPassword = ((PasswordBox)sender).Password;
    }
}