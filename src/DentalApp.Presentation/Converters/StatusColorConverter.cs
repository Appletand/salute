using System.Globalization;

namespace DentalApp.Presentation.Converters;

public class StatusColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (string)value switch
        {
            "Scheduled" => Colors.Green,
            "Completed" => Colors.Blue,
            "Cancelled" => Colors.Red,
            _ => Colors.Gray
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}