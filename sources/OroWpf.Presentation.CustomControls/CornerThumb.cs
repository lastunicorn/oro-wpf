using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DustInTheWind.OroWpf.Presentation.CustomControls;

public class CornerThumb : Thumb
{
    #region Geometry Dependency Property

    public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register(
        nameof(Geometry),
        typeof(Geometry),
        typeof(CornerThumb),
        new PropertyMetadata(null));

    public Geometry Geometry
    {
        get => (Geometry)GetValue(GeometryProperty);
        private set => SetValue(GeometryProperty, value);
    }

    #endregion

    #region CornerRadius Dependency Property

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(double),
        typeof(CornerThumb),
        new PropertyMetadata(0.1, HandleCornerRadiusPropertyChanged));

    private static void HandleCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CornerThumb cornerThumb)
            cornerThumb.UpdateVisualElements();
    }

    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    #endregion

    static CornerThumb()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CornerThumb), new FrameworkPropertyMetadata(typeof(CornerThumb)));
    }

    public CornerThumb()
    {
        UpdateVisualElements();
    }

    private void UpdateVisualElements()
    {
        Geometry = new CornerShape()
        {
            CornerType = CornerType.BottomRight,
            CornerRadius = CornerRadius
        };
    }
}
