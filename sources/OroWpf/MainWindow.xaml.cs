using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using DustInTheWind.ClockWpf.ClearClock.Controls;

namespace DustInTheWind.ClockWpf.ClearClock;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        DragMove();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ResizeGrip_DragDelta(object sender, DragDeltaEventArgs e)
    {
        double minSize = 100;

        if (MainContainer.Width == minSize && e.HorizontalChange <= 0 &&
            MainContainer.Height == minSize && e.VerticalChange <= 0)
            return;

        double newWidth = MainContainer.Width + e.HorizontalChange;
        double newHeight = MainContainer.Height + e.VerticalChange;

        double size = Math.Min(newWidth, newHeight);
        size = Math.Max(size, minSize);

        MainContainer.Width = size;
        MainContainer.Height = size;
    }
}