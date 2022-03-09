using SiliconRadarControlPanel.Converters.Base;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SiliconRadarControlPanel.Converters;

[ValueConversion(typeof(bool?), typeof(Visibility))]
[MarkupExtensionReturnType(typeof(BoolToVisibilityConverter))]
public class BoolToVisibilityConverter : ConverterBase
{
    public bool Inverted { get; set; }
    public bool Collapsed { get; set; }
        
    protected override object? Convert(object? value, Type? targetType, object? parameter, CultureInfo culture) =>
        value switch
        {
            null => null,
            Visibility => value,
            true => (Inverted is false) ? Visibility.Visible : Collapsed ? Visibility.Collapsed : Visibility.Hidden,
            false => (Inverted is true) ? Visibility.Visible : Collapsed ? Visibility.Collapsed : Visibility.Hidden,
            _ => throw new NotSupportedException()
        };

    protected override object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture) =>
        value switch
        {
            null => null,
            bool => value,
            Visibility.Visible => !Inverted,
            Visibility.Hidden => Inverted,
            Visibility.Collapsed => Inverted,
            _ => throw new NotSupportedException()
        };
}
