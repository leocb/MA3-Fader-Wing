using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using FW.Bridge.Configuration;

namespace FW.Bridge.ViewModels;
#pragma warning disable CA1822 // Mark members as static

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<DeviceViewModel> Devices { get; } = new();

    public async Task LoadConfig()
    {
        Devices.Clear();
        await Config.LoadAsync();

        Devices.AddRange(
            Config.Data.DevicesList.Select(
                d => new DeviceViewModel(d.columnOffset, d.id)
            )
        );
    }
}

#pragma warning restore CA1822 // Mark members as static
