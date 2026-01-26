using System.Globalization;
using System.Windows.Data;

namespace DustInTheWind.ClockWpf.ClearClock;

public class HalfSizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double size)
            return size / 2.0;

        return 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double size)
            return size * 2.0;

        return 0.0;
    }
}
