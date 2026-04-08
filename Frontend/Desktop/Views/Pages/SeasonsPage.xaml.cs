using System.Windows;
using System.Windows.Controls;

namespace Desktop.Views.Pages;

public partial class SeasonsPage : UserControl
{
    public SeasonsPage()
    {
        InitializeComponent();
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModels.SeasonsViewModel vm)
            await vm.LoadAsync();
    }
}