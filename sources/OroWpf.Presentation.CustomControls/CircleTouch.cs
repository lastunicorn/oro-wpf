using System.Windows;

namespace DustInTheWind.OroWpf.Presentation.CustomControls;

/// <summary>
/// Calculates the location where two circle are touching.
/// The first circle is fully defined by center (x1, y1) and radius r1.
/// For the second circle only the center is known.
/// This class calculates the location of the touch point and the radius of the second circle.
/// </summary>
internal readonly record struct CircleTouch
{
    /// <summary>
    /// Gets the x coordinate of the touch point between the two circles.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the y coordinate of the touch point between the two circles.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Gets the radius of the second circle.
    /// </summary>
    public double R2 { get; }

    public CircleTouch(double x1, double y1, double r1, double x2, double y2)
    {
        double D = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

        R2 = D / 2;
        X = (r1 * (x2 - x1)) / D + x1;
        Y = (r1 * (y2 - y1)) / D + y1;
    }

    public CircleTouch(Point c1, double r1, Point c2)
        : this(c1.X, c1.Y, r1, c2.X, c2.Y)
    {
    }

    public static implicit operator Point(CircleTouch circleTouch)
    {
        return new(circleTouch.X, circleTouch.Y);
    }
}