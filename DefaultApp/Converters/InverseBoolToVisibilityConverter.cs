using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DefaultApp.Converters;

/// <summary>
/// Converts a boolean value to Visibility, with true mapping to Collapsed and false to Visible.
/// </summary>
public sealed class InverseBoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }

        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
