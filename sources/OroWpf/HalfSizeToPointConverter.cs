using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DustInTheWind.ClockWpf.ClearClock;

public class HalfSizeToPointConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double size)
        {
            double half = size / 2.0;
            return new Point(half, half);
        }

        return new Point(0, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
