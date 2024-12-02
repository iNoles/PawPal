using System.Globalization;

namespace PawPal;

public class BoolToViewConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool v)
        {
            return v ? "Switch to Monthly View" : "Switch to Weekly View";
        }
        return "Invalid View";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
