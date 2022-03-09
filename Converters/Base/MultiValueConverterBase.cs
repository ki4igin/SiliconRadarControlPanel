using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SiliconRadarControlPanel.Converters.Base;

[MarkupExtensionReturnType(typeof(MultiValueConverterBase))]
public abstract class MultiValueConverterBase : MarkupExtension, IMultiValueConverter
{
    public override object ProvideValue(IServiceProvider sp) => this;

    protected abstract object? Convert(object[]? values, Type? targetType, object? parameter, CultureInfo culture);

    protected virtual object[] ConvertBack(object? value, Type[]? targetTypes, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException("Обратное преобразование не поддерживается");

    object? IMultiValueConverter.Convert(object[]? values, Type? targetType, object? parameter, CultureInfo culture) =>
        Convert(values, targetType, parameter, culture);

    object[] IMultiValueConverter.ConvertBack(object? value, Type[]? targetTypes, object? parameter, CultureInfo culture) =>
        ConvertBack(value, targetTypes, parameter, culture);
}