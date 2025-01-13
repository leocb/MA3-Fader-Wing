using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace FW.Bridge.Views.Converters;

public class RotationToMargin: IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var radians = ((double?) value ?? 0) + Math.PI/2;
        var ml = Math.Cos(radians) * -30;
        var mt = Math.Sin(radians) * -30;
        return new Thickness(ml,mt,0, 0);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}