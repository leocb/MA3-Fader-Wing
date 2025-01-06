using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace FW.Bridge.Views.UserControls;

public partial class DeviceButtonView : UserControl
{
    public DeviceButtonView()
    {
        InitializeComponent();
    }
    
    public static readonly StyledProperty<string?> ButtonCodeProperty =
        AvaloniaProperty.Register<DeviceButtonView,string?>(nameof(ButtonId));

    public string? ButtonId
    {
        get => GetValue(ButtonCodeProperty);
        set => SetValue(ButtonCodeProperty, value);
    }
    
    
    public static readonly StyledProperty<bool> IsPressedProperty =
        AvaloniaProperty.Register<DeviceButtonView,bool>(nameof(IsPressed));
    
    public bool IsPressed
    {
        get => GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }
}