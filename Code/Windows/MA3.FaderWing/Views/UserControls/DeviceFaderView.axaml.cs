using Avalonia;
using Avalonia.Controls;

namespace FW.Bridge.Views.UserControls;

public partial class DeviceFaderView : UserControl
{
    public DeviceFaderView()
    {
        InitializeComponent();
        AffectsRender<DeviceButtonView>(FadeCurrentProperty);
    }
    
    public static readonly StyledProperty<double> FadeCurrentProperty =
        AvaloniaProperty.Register<DeviceButtonView,double>(nameof(FadeCurrent));
    
    public double FadeCurrent
    {
        get => GetValue(FadeCurrentProperty);
        set => SetValue(FadeCurrentProperty, value);
    }
    
    public static readonly StyledProperty<double> FadeTargetProperty =
        AvaloniaProperty.Register<DeviceButtonView,double>(nameof(FadeTarget));
    
    public double FadeTarget
    {
        get => GetValue(FadeTargetProperty);
        set => SetValue(FadeTargetProperty, value);
    }
}