using System.Windows;
using System.Windows.Controls;

namespace Desktop.Views.Pages;

public partial class RatesPage : UserControl
{
    public RatesPage()
    {
        InitializeComponent();
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModels.RatesViewModel vm)
            await vm.LoadAsync();
    }
}