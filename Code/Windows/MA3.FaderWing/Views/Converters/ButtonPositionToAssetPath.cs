using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FW.Bridge.Data;

namespace FW.Bridge.Views.Converters;

public class ButtonPositionToAssetPathA: IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var pos = (ButtonPosition?) value ?? ButtonPosition.TL;
        var path = pos switch
        {
            ButtonPosition.TL => $"avares://FW.Bridge/Assets/Device/Cuts/Device Button Top Left A.png",
            ButtonPosition.TC => $"avares://FW.Bridge/Assets/Device/Cuts/Device Button Top Center A.png",
            ButtonPosition.TR => $"avares://FW.Bridge/Assets/Device/Cuts/Device Button Top Right A.png",
            ButtonPosition.ML => $"avares://FW.Bridge/Assets/Device/Cuts/Device Button Middle Left A.png",
            ButtonPosition.MC => $"avares://FW.Bridge/Assets/Device/Cuts/Device Button Middle Center A.png",
            ButtonPosition.MR => $"avares://FW.Bridge/Assets/Device/Cuts/Device Button Middle Right A.png",
            ButtonPosition.BL => $"avares://FW.Bridge/Assets/Device/Cuts/Device Button Bottom Left A.png",
            ButtonPosition.BC => $"avares://FW.Bridge/Assets/Device/Cuts/Device Button Bottom Center A.png",
            ButtonPosition.BR => $"avares://FW.Bridge/Assets/Device/Cuts/Device Button Bottom Right A.png",
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
        return new Bitmap(AssetLoader.Open(new Uri(path)));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class ButtonPositionToAssetPathB: IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var pos = (ButtonPosition?) value ?? ButtonPosition.TL;
        var path = pos switch
        {
            ButtonPosition.TL => "avares://FW.Bridge/Assets/Device/Cuts/Device Button Top Left B.png",
            ButtonPosition.TC => "avares://FW.Bridge/Assets/Device/Cuts/Device Button Top Center B.png",
            ButtonPosition.TR => "avares://FW.Bridge/Assets/Device/Cuts/Device Button Top Right B.png",
            ButtonPosition.ML => "avares://FW.Bridge/Assets/Device/Cuts/Device Button Middle Left B.png",
            ButtonPosition.MC => "avares://FW.Bridge/Assets/Device/Cuts/Device Button Middle Center B.png",
            ButtonPosition.MR => "avares://FW.Bridge/Assets/Device/Cuts/Device Button Middle Right B.png",
            ButtonPosition.BL => "avares://FW.Bridge/Assets/Device/Cuts/Device Button Bottom Left B.png",
            ButtonPosition.BC => "avares://FW.Bridge/Assets/Device/Cuts/Device Button Bottom Center B.png",
            ButtonPosition.BR => "avares://FW.Bridge/Assets/Device/Cuts/Device Button Bottom Right B.png",
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
        return new Bitmap(AssetLoader.Open(new Uri(path)));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}