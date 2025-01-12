using Avalonia;
using Avalonia.Controls;

namespace FW.Bridge.Views.UserControls;

public partial class DeviceRotaryView : UserControl
{
    public DeviceRotaryView()
    {
        InitializeComponent();
        AffectsRender<DeviceButtonView>(IsPressedProperty, RotationProperty);
    }
    
    public static readonly StyledProperty<bool> IsPressedProperty =
        AvaloniaProperty.Register<DeviceButtonView,bool>(nameof(IsPressed));
    
    public bool IsPressed
    {
        get => GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }
    
    public static readonly StyledProperty<double> RotationProperty =
        AvaloniaProperty.Register<DeviceButtonView,double>(nameof(RotationAmount));
    
    public double RotationAmount
    {
        get => GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }
}