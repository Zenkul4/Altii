using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Desktop.ViewModels;

namespace Desktop.Views;

public partial class MainShellView : Window
{
    private DispatcherTimer? _clock;

    public MainShellView(MainShellViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;

        Loaded += (s, e) => vm.UpdateSessionInfo();

        _clock = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _clock.Tick += (_, _) =>
        {
            if (ClockText is not null)
                ClockText.Text = DateTime.Now.ToString("dddd dd/MM/yyyy  HH:mm:ss");
        };
        _clock.Start();
    }

    private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            try { DragMove(); } catch { }
    }
}