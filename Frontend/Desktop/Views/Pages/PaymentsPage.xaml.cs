using System.Windows;
using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop.Views.Pages;

public partial class PaymentsPage : UserControl
{
    public PaymentsPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is PaymentsViewModel vm)
            _ = vm.LoadAsync();
    }
}