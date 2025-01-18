using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using HidSharp;

namespace FW.Bridge.USB;

public static class DeviceManager
{
    public delegate void DeviceManagerEventsHandler();
    public static event DeviceManagerEventsHandler? OnChanged;
    
    public static Dictionary<string,HidDevice> AvailableDevices { get; private set; }
    
    static DeviceManager()
    {
        AvailableDevices = GetHidDevicesSerial();

        // Throttle HID changed events, this prevents trying to connect to
        // now disconnecting devices due to repeated HID entries
        Observable
            .FromEventPattern<DeviceListChangedEventArgs>(
                DeviceList.Local,
                nameof(DeviceList.Local.Changed))
            .Sample(TimeSpan.FromMilliseconds(500))
            .Subscribe(o =>
            {
                AvailableDevices = GetHidDevicesSerial();
                OnChanged?.Invoke();
            });
    }

    // Find HID devices.
    public static Dictionary<string, HidDevice> GetHidDevicesSerial()
    {
        var devices = DeviceList.Local.GetHidDevices();
        var filteredDevices = devices.Where(d =>
        {
            try
            {
                return d.GetManufacturer() == "github.com/leocb";
            }
            catch
            {
                return false;
            }
        });
        var outputDict = filteredDevices.ToDictionary(d => d.GetSerialNumber(), d => d);
        return outputDict;
    }
}