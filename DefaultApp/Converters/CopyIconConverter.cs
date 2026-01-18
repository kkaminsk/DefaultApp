using Microsoft.UI.Xaml.Data;

namespace DefaultApp.Converters;

/// <summary>
/// Converts the copied field name to the appropriate glyph.
/// Shows a checkmark when the field was just copied, otherwise shows the copy icon.
/// </summary>
public sealed class CopyIconConverter : IValueConverter
{
    /// <summary>
    /// Copy icon glyph (E8C8).
    /// </summary>
    public const string CopyGlyph = "\uE8C8";

    /// <summary>
    /// Checkmark icon glyph (E73E).
    /// </summary>
    public const string CheckmarkGlyph = "\uE73E";

    /// <summary>
    /// Converts the copied field name to a glyph.
    /// </summary>
    /// <param name="value">The current CopiedFieldName from the ViewModel.</param>
    /// <param name="targetType">The target type (string).</param>
    /// <param name="parameter">The field name this button is associated with.</param>
    /// <param name="language">The language.</param>
    /// <returns>Checkmark glyph if the field was just copied, otherwise copy glyph.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string copiedFieldName && parameter is string fieldName)
        {
            return string.Equals(copiedFieldName, fieldName, StringComparison.Ordinal)
                ? CheckmarkGlyph
                : CopyGlyph;
        }

        return CopyGlyph;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
