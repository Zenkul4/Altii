using System.Windows;
using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop.Views;

public partial class LoginView : Window
{
    public LoginView(LoginViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;

        MouseLeftButtonDown += (_, e) => DragMove();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
            vm.Password = ((PasswordBox)sender).Password;
    }
}