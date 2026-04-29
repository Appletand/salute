using System.Globalization;

namespace Salute.Converters;

public class BooleanConverter : IValueConverter
{
    public bool Invert { get; set; } = false;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
            return Invert ? !b : b;
        return !Invert;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
            return Invert ? !b : b;
        return !Invert;
    }
}