using System.Windows;
using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop.Views.Pages;

public partial class BookingsPage : UserControl
{
    public BookingsPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is BookingsViewModel vm)
            _ = vm.LoadAsync();
    }
}