using System.Windows;
using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop.Views.Pages;

public partial class RoomsPage : UserControl
{
    public RoomsPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is RoomsViewModel vm)
            _ = vm.LoadAsync();
    }
}