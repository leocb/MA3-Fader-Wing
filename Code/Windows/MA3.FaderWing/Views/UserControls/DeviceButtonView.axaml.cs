using Avalonia;
using Avalonia.Controls;
using FW.Bridge.Data;

namespace FW.Bridge.Views.UserControls;

public partial class DeviceButtonView : UserControl
{
    public DeviceButtonView()
    {
        InitializeComponent();
        AffectsRender<DeviceButtonView>(ButtonIdProperty, IsPressedProperty);
    }
    
    public static readonly StyledProperty<string?> ButtonIdProperty =
        AvaloniaProperty.Register<DeviceButtonView,string?>(nameof(ButtonId),"ND");

    public string? ButtonId
    {
        get => GetValue(ButtonIdProperty);
        set => SetValue(ButtonIdProperty, value);
    }
    
    
    public static readonly StyledProperty<bool> IsPressedProperty =
        AvaloniaProperty.Register<DeviceButtonView,bool>(nameof(IsPresed));
    
    public bool IsPresed
    {
        get => GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }
    
    
    public static readonly StyledProperty<ButtonPosition> ButtonPositionProperty =
        AvaloniaProperty.Register<DeviceButtonView,ButtonPosition>(nameof(Asset));
    
    public ButtonPosition Asset
    {
        get => GetValue(ButtonPositionProperty);
        set => SetValue(ButtonPositionProperty, value);
    }
}