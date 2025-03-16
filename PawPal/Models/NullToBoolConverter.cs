using System.Globalization;

namespace PawPal.Models;

public class NullToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // If the value is null, return false, otherwise return true.
        return value != null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // If the value is false, return null, otherwise return a non-null value (true).
        return (value is bool v && v) ? new object() : null;
    }
}
