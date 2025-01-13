using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FW.Bridge.ViewModels;
using HidSharp;
using HidSharp.Reports;
using HidSharp.Reports.Input;

namespace FW.Bridge.USB;

public class DeviceControl
{
    private DeviceViewModel _vm;

    public DeviceControl(DeviceViewModel vm)
    {
        _vm = vm;
        EnumerateHidDevices();
    }

    // Find HID devices.
    private void EnumerateHidDevices()
    {
        var a = DeviceList.Local.GetHidDevices();
        HidDevice device = a
            .First(d => d.DevicePath.Equals(
                @"\\?\hid#vid_303a&pid_0002&mi_00#7&8fdf634&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}"));

        StartReadingInputs(device);

        DeviceList.Local.Changed += (device, args) =>
        {
            Console.WriteLine("Device list changed!");
        };
    }

    private void StartReadingInputs(HidDevice device)
    {
        if (!device.TryOpen(out HidStream hidStream)) return;

        Console.WriteLine($"Opened device {device.DevicePath}");
        hidStream.ReadTimeout = Timeout.Infinite;

        var inputReportBuffer = new byte[device.GetMaxInputReportLength()];
        var inputReceiver = device.GetReportDescriptor().CreateHidDeviceInputReceiver();
            
        var deviceItem = device.GetReportDescriptor().DeviceItems[0]; // Joystick collection
        var inputParser = deviceItem.CreateDeviceItemInputParser();

        inputReceiver.Start(hidStream);
        
        Task.Run(void() =>
        {
            while (true)
            {
                if (!inputReceiver.IsRunning) break; // Disconnected?
                if (!inputReceiver.WaitHandle.WaitOne(1000)) continue;
                if (!inputReceiver.TryRead(inputReportBuffer, 0, out var report)) continue;
                if (!inputParser.TryParseReport(inputReportBuffer, 0, report)) continue;

                ParseHidData(inputParser);
            }
            hidStream.Dispose();
        });
    }

    private const int ROTARY_PUSH_OFFSET = 0;
    private const int BUTTON300_OFFSET = 5;
    private const int BUTTON200_OFFSET = 10;
    private const int BUTTON100_OFFSET = 15;
    private const int FADER_OFFSET = 20;
    private const int ROTARY_ROT_OFFSET = 25;
    
    private void ParseHidData(DeviceItemInputParser parser)
    {
        if (!parser.HasChanged) return;
        for (int valueIndex = 0; valueIndex < parser.ValueCount; valueIndex++)
        {
            var dataValue = parser.GetValue(valueIndex);
            Console.WriteLine("{0}: {1}", (Usage)dataValue.Usages.FirstOrDefault(), dataValue.GetLogicalValue());
        }

        _vm.Rotary1Push = parser.GetValue(ROTARY_PUSH_OFFSET + 0).GetLogicalValue() == 1;
        _vm.Rotary2Push = parser.GetValue(ROTARY_PUSH_OFFSET + 1).GetLogicalValue() == 1;
        _vm.Rotary3Push = parser.GetValue(ROTARY_PUSH_OFFSET + 2).GetLogicalValue() == 1;
        _vm.Rotary4Push = parser.GetValue(ROTARY_PUSH_OFFSET + 3).GetLogicalValue() == 1;
        _vm.Rotary5Push = parser.GetValue(ROTARY_PUSH_OFFSET + 4).GetLogicalValue() == 1;
        
        _vm.Button301Push = parser.GetValue(BUTTON300_OFFSET + 0).GetLogicalValue() == 1;
        _vm.Button302Push = parser.GetValue(BUTTON300_OFFSET + 1).GetLogicalValue() == 1;
        _vm.Button303Push = parser.GetValue(BUTTON300_OFFSET + 2).GetLogicalValue() == 1;
        _vm.Button304Push = parser.GetValue(BUTTON300_OFFSET + 3).GetLogicalValue() == 1;
        _vm.Button305Push = parser.GetValue(BUTTON300_OFFSET + 4).GetLogicalValue() == 1;
        
        _vm.Button201Push = parser.GetValue(BUTTON200_OFFSET + 0).GetLogicalValue() == 1;
        _vm.Button202Push = parser.GetValue(BUTTON200_OFFSET + 1).GetLogicalValue() == 1;
        _vm.Button203Push = parser.GetValue(BUTTON200_OFFSET + 2).GetLogicalValue() == 1;
        _vm.Button204Push = parser.GetValue(BUTTON200_OFFSET + 3).GetLogicalValue() == 1;
        _vm.Button205Push = parser.GetValue(BUTTON200_OFFSET + 4).GetLogicalValue() == 1;
        
        _vm.Button101Push = parser.GetValue(BUTTON100_OFFSET + 0).GetLogicalValue() == 1;
        _vm.Button102Push = parser.GetValue(BUTTON100_OFFSET + 1).GetLogicalValue() == 1;
        _vm.Button103Push = parser.GetValue(BUTTON100_OFFSET + 2).GetLogicalValue() == 1;
        _vm.Button104Push = parser.GetValue(BUTTON100_OFFSET + 3).GetLogicalValue() == 1;
        _vm.Button105Push = parser.GetValue(BUTTON100_OFFSET + 4).GetLogicalValue() == 1;
        
        _vm.Fader1Cur = parser.GetValue(FADER_OFFSET + 0).GetLogicalValue() / (double)255;
        _vm.Fader2Cur = parser.GetValue(FADER_OFFSET + 1).GetLogicalValue() / (double)255;
        _vm.Fader3Cur = parser.GetValue(FADER_OFFSET + 2).GetLogicalValue() / (double)255;
        _vm.Fader4Cur = parser.GetValue(FADER_OFFSET + 3).GetLogicalValue() / (double)255;
        _vm.Fader5Cur = parser.GetValue(FADER_OFFSET + 4).GetLogicalValue() / (double)255;
        
        _vm.Rotary1Rot += parser.GetValue(ROTARY_ROT_OFFSET + 0).GetLogicalValue() - 1;
        _vm.Rotary2Rot += parser.GetValue(ROTARY_ROT_OFFSET + 1).GetLogicalValue() - 1;
        _vm.Rotary3Rot += parser.GetValue(ROTARY_ROT_OFFSET + 2).GetLogicalValue() - 1;
        _vm.Rotary4Rot += parser.GetValue(ROTARY_ROT_OFFSET + 3).GetLogicalValue() - 1;
        _vm.Rotary5Rot += parser.GetValue(ROTARY_ROT_OFFSET + 4).GetLogicalValue() - 1;
    }
}
