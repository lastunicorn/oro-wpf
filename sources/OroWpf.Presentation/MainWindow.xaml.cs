using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using DustInTheWind.OroWpf.Ports.SettingsAccess;

namespace DustInTheWind.OroWpf.Presentation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ISettings settings;

    public MainWindow()
    {
        InitializeComponent();

        Loaded += MainWindow_Loaded;
        LocationChanged += MainWindow_LocationChanged;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            settings = viewModel.Settings;
            LoadWindowLocation();
        }
    }

    private void LoadWindowLocation()
    {
        if (settings != null)
        {
            double left = settings.WindowLeft;
            double top = settings.WindowTop;

            if (!double.IsNaN(left) && !double.IsNaN(top))
            {
                Left = left;
                Top = top;
                EnsureWindowIsOnScreen();
            }
        }
    }

    private void EnsureWindowIsOnScreen()
    {
        double windowWidth = ActualWidth;
        double windowHeight = ActualHeight;

        if (windowWidth == 0)
            windowWidth = Width;
        if (windowHeight == 0)
            windowHeight = Height;

        double virtualScreenLeft = SystemParameters.VirtualScreenLeft;
        double virtualScreenTop = SystemParameters.VirtualScreenTop;
        double virtualScreenWidth = SystemParameters.VirtualScreenWidth;
        double virtualScreenHeight = SystemParameters.VirtualScreenHeight;

        bool isCompletelyOutside =
            Left + windowWidth < virtualScreenLeft ||
            Left > virtualScreenLeft + virtualScreenWidth ||
            Top + windowHeight < virtualScreenTop ||
            Top > virtualScreenTop + virtualScreenHeight;

        if (isCompletelyOutside)
        {
            double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
            double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
            Left = (primaryScreenWidth - windowWidth) / 2;
            Top = (primaryScreenHeight - windowHeight) / 2;
        }
        else
        {
            if (Left < virtualScreenLeft)
                Left = virtualScreenLeft;
            else if (Left + windowWidth > virtualScreenLeft + virtualScreenWidth)
                Left = virtualScreenLeft + virtualScreenWidth - windowWidth;

            if (Top < virtualScreenTop)
                Top = virtualScreenTop;
            else if (Top + windowHeight > virtualScreenTop + virtualScreenHeight)
                Top = virtualScreenTop + virtualScreenHeight - windowHeight;
        }
    }

    private void MainWindow_LocationChanged(object sender, EventArgs e)
    {
        if (settings != null && IsLoaded)
            settings.SetWindowLocation(Left, Top);
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