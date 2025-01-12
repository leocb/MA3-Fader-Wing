using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.Converters;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FW.Bridge.Data;

namespace FW.Bridge.Views.Converters;

public class FadeAmountToMargin: IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var amount = (double?) value ?? 0;
        return new Thickness(0,0,0,amount * 194);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}