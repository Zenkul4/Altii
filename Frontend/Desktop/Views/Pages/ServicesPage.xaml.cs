using System.Windows;
using System.Windows.Controls;

namespace Desktop.Views.Pages;

public partial class ServicesPage : UserControl
{
    public ServicesPage()
    {
        InitializeComponent();
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModels.ServicesViewModel vm)
            await vm.LoadAsync();
    }
}