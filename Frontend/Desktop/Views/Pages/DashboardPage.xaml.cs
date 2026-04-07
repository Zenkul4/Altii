using System.Windows;
using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop.Views.Pages;

public partial class DashboardPage : UserControl
{
    public DashboardPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is DashboardViewModel vm)
            _ = vm.LoadAsync();
    }
}