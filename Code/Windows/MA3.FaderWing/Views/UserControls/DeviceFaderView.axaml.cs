using Avalonia;
using Avalonia.Controls;

namespace FW.Bridge.Views.UserControls;

public partial class DeviceFaderView : UserControl
{
    public DeviceFaderView()
    {
        InitializeComponent();
        AffectsRender<DeviceButtonView>(FadeAmountProperty);
    }
    
    public static readonly StyledProperty<double> FadeAmountProperty =
        AvaloniaProperty.Register<DeviceButtonView,double>(nameof(FadeCurrent));
    
    public double FadeCurrent
    {
        get => GetValue(FadeAmountProperty);
        set => SetValue(FadeAmountProperty, value);
    }
    
    public static readonly StyledProperty<double> FadeTargetProperty =
        AvaloniaProperty.Register<DeviceButtonView,double>(nameof(FadeTarget));
    
    public double FadeTarget
    {
        get => GetValue(FadeTargetProperty);
        set => SetValue(FadeTargetProperty, value);
    }
}