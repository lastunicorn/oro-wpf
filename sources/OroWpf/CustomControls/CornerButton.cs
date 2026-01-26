using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DustInTheWind.ClockWpf.ClearClock.CustomControls;

public class CornerButton : Button
{
    #region CornerType Dependency Property

    public static readonly DependencyProperty CornerTypeProperty = DependencyProperty.Register(
        nameof(Corner),
        typeof(CornerType),
        typeof(CornerButton),
        new PropertyMetadata(CornerType.TopLeft, HandlePropertyChanged));

    private static void HandlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CornerButton cornerButton)
            cornerButton.UpdateVisualElements();
    }

    public CornerType Corner
    {
        get => (CornerType)GetValue(CornerTypeProperty);
        set => SetValue(CornerTypeProperty, value);
    }

    #endregion

    #region CornerRadius Dependency Property

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(double),
        typeof(CornerButton),
        new PropertyMetadata(0.1, HandleCornerRadiusPropertyChanged));

    private static void HandleCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CornerButton cornerButton)
            cornerButton.UpdateVisualElements();
    }

    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    #endregion

    #region Geometry Dependency Property

    public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register(
        nameof(Geometry),
        typeof(Geometry),
        typeof(CornerButton),
        new PropertyMetadata(null));

    public Geometry Geometry
    {
        get => (Geometry)GetValue(GeometryProperty);
        private set => SetValue(GeometryProperty, value);
    }

    #endregion

    #region ContentHorizontalAlignment Dependency Property

    public static readonly DependencyProperty ContentHorizontalAlignmentProperty = DependencyProperty.Register(
        nameof(ContentHorizontalAlignment),
        typeof(HorizontalAlignment),
        typeof(CornerButton),
        new PropertyMetadata(HorizontalAlignment.Left));

    public HorizontalAlignment ContentHorizontalAlignment
    {
        get => (HorizontalAlignment)GetValue(ContentHorizontalAlignmentProperty);
        set => SetValue(ContentHorizontalAlignmentProperty, value);
    }

    #endregion

    #region ContentVerticalAlignment Dependency Property

    public static readonly DependencyProperty ContentVerticalAlignmentProperty = DependencyProperty.Register(
        nameof(ContentVerticalAlignment),
        typeof(VerticalAlignment),
        typeof(CornerButton),
        new PropertyMetadata(VerticalAlignment.Top));

    public VerticalAlignment ContentVerticalAlignment
    {
        get => (VerticalAlignment)GetValue(ContentVerticalAlignmentProperty);
        set => SetValue(ContentVerticalAlignmentProperty, value);
    }

    #endregion

    #region HoverBackground Dependency Property

    public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.Register(
        nameof(HoverBackground),
        typeof(Brush),
        typeof(CornerButton),
        new PropertyMetadata(null));

    public Brush HoverBackground
    {
        get => (Brush)GetValue(HoverBackgroundProperty);
        set => SetValue(HoverBackgroundProperty, value);
    }

    #endregion

    static CornerButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CornerButton), new FrameworkPropertyMetadata(typeof(CornerButton)));
    }

    public CornerButton()
    {
        UpdateVisualElements();
    }

    private void UpdateVisualElements()
    {
        Geometry = new CornerShape()
        {
            CornerType = Corner,
            CornerRadius = CornerRadius
        };

        switch (Corner)
        {
            case CornerType.TopLeft:
                ContentHorizontalAlignment = HorizontalAlignment.Left;
                ContentVerticalAlignment = VerticalAlignment.Top;

                break;

            case CornerType.TopRight:
                ContentHorizontalAlignment = HorizontalAlignment.Right;
                ContentVerticalAlignment = VerticalAlignment.Top;
                break;

            case CornerType.BottomLeft:
                ContentHorizontalAlignment = HorizontalAlignment.Left;
                ContentVerticalAlignment = VerticalAlignment.Bottom;
                break;

            case CornerType.BottomRight:
                ContentHorizontalAlignment = HorizontalAlignment.Right;
                ContentVerticalAlignment = VerticalAlignment.Bottom;
                break;

            default:
                return;
        }
    }
}
