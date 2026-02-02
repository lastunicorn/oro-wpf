using System.Windows;
using DustInTheWind.OroWpf.Ports.SettingsAccess;

namespace DustInTheWind.OroWpf.Presentation.Behaviors;

public static class WindowLocationBehavior
{
    #region Settings Attached Property

    public static readonly DependencyProperty SettingsProperty = DependencyProperty.RegisterAttached(
        "Settings",
        typeof(ISettings),
        typeof(WindowLocationBehavior),
        new PropertyMetadata(null, OnSettingsChanged));

    public static ISettings GetSettings(DependencyObject obj)
    {
        return (ISettings)obj.GetValue(SettingsProperty);
    }

    public static void SetSettings(DependencyObject obj, ISettings value)
    {
        obj.SetValue(SettingsProperty, value);
    }

    private static void OnSettingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
        {
            if (e.OldValue != null)
            {
                window.SourceInitialized -= Window_SourceInitialized;
                window.LocationChanged -= Window_LocationChanged;
            }

            if (e.NewValue != null)
            {
                window.SourceInitialized += Window_SourceInitialized;
                window.LocationChanged += Window_LocationChanged;
            }
        }
    }

    #endregion

    #region ContainerElement Attached Property

    public static readonly DependencyProperty ContainerElementProperty = DependencyProperty.RegisterAttached(
        "ContainerElement",
        typeof(FrameworkElement),
        typeof(WindowLocationBehavior),
        new PropertyMetadata(null, OnContainerElementChanged));

    public static FrameworkElement GetContainerElement(DependencyObject obj)
    {
        return (FrameworkElement)obj.GetValue(ContainerElementProperty);
    }

    public static void SetContainerElement(DependencyObject obj, FrameworkElement value)
    {
        obj.SetValue(ContainerElementProperty, value);
    }

    private static void OnContainerElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
        {
            if (e.OldValue is FrameworkElement oldElement)
                oldElement.SizeChanged -= ContainerElement_SizeChanged;

            if (e.NewValue is FrameworkElement newElement)
                newElement.SizeChanged += ContainerElement_SizeChanged;
        }
    }

    #endregion

    private static void Window_SourceInitialized(object sender, EventArgs e)
    {
        if (sender is Window window)
        {
            ISettings settings = GetSettings(window);
            if (settings != null)
            {
                LoadWindowLocation(window, settings);

                FrameworkElement container = GetContainerElement(window);
                if (container != null)
                {
                    LoadContainerSize(container, settings);
                }

                window.ContentRendered += Window_ContentRendered;
            }
        }
    }

    private static void Window_ContentRendered(object sender, EventArgs e)
    {
        if (sender is Window window)
        {
            window.ContentRendered -= Window_ContentRendered;
            EnsureWindowIsOnScreen(window);
        }
    }

    private static void Window_LocationChanged(object sender, EventArgs e)
    {
        if (sender is Window window && window.IsLoaded)
        {
            ISettings settings = GetSettings(window);

            if (settings != null)
                settings.SetWindowLocation(window.Left, window.Top);
        }
    }

    private static void ContainerElement_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (sender is FrameworkElement container)
        {
            Window window = Window.GetWindow(container);
            if (window != null && window.IsLoaded)
            {
                ISettings settings = GetSettings(window);

                if (settings != null)
                    settings.SetWindowSize(container.ActualWidth, container.ActualHeight);
            }
        }
    }

    private static void LoadWindowLocation(Window window, ISettings settings)
    {
        double left = settings.WindowLeft;
        double top = settings.WindowTop;

        if (!double.IsNaN(left) && !double.IsNaN(top))
        {
            window.Left = left;
            window.Top = top;
        }
    }

    private static void LoadContainerSize(FrameworkElement container, ISettings settings)
    {
        double width = settings.WindowWidth;
        double height = settings.WindowHeight;

        if (!double.IsNaN(width) && !double.IsNaN(height))
        {
            container.Width = width;
            container.Height = height;
        }
    }

    private static void EnsureWindowIsOnScreen(Window window)
    {
        double windowWidth = window.ActualWidth;
        double windowHeight = window.ActualHeight;

        if (windowWidth == 0)
            windowWidth = window.Width;

        if (windowHeight == 0)
            windowHeight = window.Height;

        if (windowWidth == 0 || double.IsNaN(windowWidth))
            return;

        if (windowHeight == 0 || double.IsNaN(windowHeight))
            return;

        double virtualScreenLeft = SystemParameters.VirtualScreenLeft;
        double virtualScreenTop = SystemParameters.VirtualScreenTop;
        double virtualScreenWidth = SystemParameters.VirtualScreenWidth;
        double virtualScreenHeight = SystemParameters.VirtualScreenHeight;

        bool isCompletelyOutside =
            window.Left + windowWidth < virtualScreenLeft ||
            window.Left > virtualScreenLeft + virtualScreenWidth ||
            window.Top + windowHeight < virtualScreenTop ||
            window.Top > virtualScreenTop + virtualScreenHeight;

        if (isCompletelyOutside)
        {
            double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
            double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
            window.Left = (primaryScreenWidth - windowWidth) / 2;
            window.Top = (primaryScreenHeight - windowHeight) / 2;
        }
        else
        {
            if (window.Left < virtualScreenLeft)
                window.Left = virtualScreenLeft;
            else if (window.Left + windowWidth > virtualScreenLeft + virtualScreenWidth)
                window.Left = virtualScreenLeft + virtualScreenWidth - windowWidth;

            if (window.Top < virtualScreenTop)
                window.Top = virtualScreenTop;
            else if (window.Top + windowHeight > virtualScreenTop + virtualScreenHeight)
                window.Top = virtualScreenTop + virtualScreenHeight - windowHeight;
        }
    }
}
