using System.Windows;
using System.Windows.Input;

namespace DustInTheWind.OroWpf.Presentation.MainArea;

/// <summary>
/// Adds a drag behavior to a <see cref="Window"/> that allows the user to move the 
/// window on the screen.
/// </summary>
public static class WindowDragBehavior
{
    public static readonly DependencyProperty EnableDragProperty = DependencyProperty.RegisterAttached(
        "EnableDrag",
        typeof(bool),
        typeof(WindowDragBehavior),
        new PropertyMetadata(false, OnEnableDragChanged));

    public static bool GetEnableDrag(DependencyObject obj)
    {
        return (bool)obj.GetValue(EnableDragProperty);
    }

    public static void SetEnableDrag(DependencyObject obj, bool value)
    {
        obj.SetValue(EnableDragProperty, value);
    }

    private static void OnEnableDragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
        {
            if ((bool)e.NewValue)
                window.MouseLeftButtonDown += HandleMouseLeftButtonDown;
            else
                window.MouseLeftButtonDown -= HandleMouseLeftButtonDown;
        }
    }

    private static void HandleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is Window window)
            window.DragMove();
    }
}
