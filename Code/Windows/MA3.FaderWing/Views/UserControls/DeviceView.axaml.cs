using Avalonia.Controls;
using FW.Bridge.ViewModels;

namespace FW.Bridge.Views.UserControls;

public partial class DeviceView : UserControl
{
    public DeviceView()
    {
        InitializeComponent();
        DataContext = new DeviceViewModel(0, "");
    }
}