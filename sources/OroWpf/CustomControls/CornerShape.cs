using System.Windows;
using System.Windows.Media;

namespace DustInTheWind.ClockWpf.ClearClock.CustomControls;

internal class CornerShape
{
    public CornerType CornerType { get; set; }

    public double CornerRadius { get; set; }

    public Geometry ToGeometry()
    {
        PathFigure pathFigure = GenerateTopLeftPathFigure(CornerRadius);

        PathGeometry geometry = new()
        {
            Figures =
            {
                pathFigure
            }
        };

        geometry.Transform = CornerType switch
        {
            CornerType.TopLeft => null,
            CornerType.TopRight => new RotateTransform(90, 0.5, 0.5),
            CornerType.BottomRight => new RotateTransform(180, 0.5, 0.5),
            CornerType.BottomLeft => new RotateTransform(270, 0.5, 0.5),
            _ => null
        };

        return geometry;
    }

    private static PathFigure GenerateTopLeftPathFigure(double cornerRadius = 0.1)
    {
        // size = 1 x 1
        // corner radius = 0.1
        //
        // M 0.1865,0.9501 - Starts from bottom-left corner
        // A 0.1,0.1 0 0 1 0,0.9 - Draw the arc of the bottom-left corner
        // L 0,0.1 - Draw line to top-left corner
        // A 0.1,0.1 0 0 1 0.1,0 - Draw the arc of the top-left corner
        // L 0.9,0 - Draw line to top-right corner
        // A 0.1,0.1 0 0 1 0.9501,0.1865 - Draw arc of the top-right corner
        // A 2,2 0 0 1 0.1,1 - Draw big arc from top-right corner to bottom-left corner
        // Z - Close the path

        CircleTouch touchPointTopRight = new(1 - cornerRadius, cornerRadius, cornerRadius, 2, 2);
        CircleTouch touchPointBottomLeft = new(cornerRadius, 1 - cornerRadius, cornerRadius, 2, 2);

        return new PathFigure()
        {
            // Starts from bottom-left corner
            StartPoint = touchPointBottomLeft,
            IsClosed = true,
            Segments =
            {
                // Draw the arc of the bottom-left corner
                new ArcSegment
                {
                    Point = new Point(0, 1 - cornerRadius),
                    Size = new Size(cornerRadius, cornerRadius),
                    RotationAngle = 0,
                    IsLargeArc = false,
                    SweepDirection = SweepDirection.Clockwise
                },

                // Draw line to top-left corner
                new LineSegment
                {
                    Point = new Point(0, cornerRadius)
                },

                // Draw the arc of the top-left corner
                new ArcSegment
                {
                    Point = new Point(cornerRadius, 0),
                    Size = new Size(cornerRadius, cornerRadius),
                    RotationAngle = 0,
                    IsLargeArc = false,
                    SweepDirection = SweepDirection.Clockwise
                },

                // Draw line to top-right corner
                new LineSegment
                {
                    Point = new Point(1 - cornerRadius, 0)
                },

                // Draw arc of the top-right corner
                new ArcSegment
                {
                    Point = touchPointTopRight,
                    Size = new Size(cornerRadius, cornerRadius),
                    RotationAngle = 0,
                    IsLargeArc = false,
                    SweepDirection = SweepDirection.Clockwise
                },

                // Draw big arc from top-right corner to bottom-left corner
                new ArcSegment
                {
                    Point = touchPointBottomLeft,
                    Size = new Size(2, 2),
                    RotationAngle = 0,
                    IsLargeArc = false,
                    SweepDirection = SweepDirection.Counterclockwise
                }
            }
        };
    }

    public static implicit operator Geometry(CornerShape cornerShape)
    {
        return cornerShape.ToGeometry();
    }
}