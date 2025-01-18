using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FW.Bridge.ViewModels;
using HidSharp;
using HidSharp.Reports;
using HidSharp.Reports.Input;

namespace FW.Bridge.USB;

public class DeviceController
{
    private const bool DebugHidValues = false;
    
    private DeviceViewModel _vm;

    public DeviceController(DeviceViewModel vm)
    {
        _vm = vm;
        
        // Auto connect to device with same serial number.
        DeviceManager.OnChanged += () =>
        {
            if (DeviceIsPresent())
                ConnectToHidDevice();
            else
                _vm.IsConnected = false;
        };

        if (DeviceIsPresent())
        {
            ConnectToHidDevice();
        }

    }

    private bool DeviceIsPresent() => DeviceManager.AvailableDevices.ContainsKey(_vm.Serial);

    private void ConnectToHidDevice()
    {
        try
        {
            _vm.IsConnected = false;
            if (!DeviceManager.AvailableDevices.TryGetValue(_vm.Serial, out HidDevice? device))
                return;
            _ = device ?? throw new NullReferenceException($"Invalid Device Instance. SN:{_vm.Serial}");
            
            StartReadingInputs(device);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error connecting to device: {ex.Message}");
        }
    }
    
    private void StartReadingInputs(HidDevice device)
    {
        if (!device.TryOpen(out HidStream hidStream))
            throw new IOException("Could not open HID device stream");

        hidStream.ReadTimeout = Timeout.Infinite;

        var inputReportBuffer = new byte[device.GetMaxInputReportLength()];
        var inputReceiver = device.GetReportDescriptor().CreateHidDeviceInputReceiver();
            
        var deviceItem = device.GetReportDescriptor().DeviceItems[0]; // Joystick collection
        var inputParser = deviceItem.CreateDeviceItemInputParser();

        inputReceiver.Start(hidStream);
        
        // This task keeps running in the background,
        // it returns when the input receiver stops running (device disconnected)
        Task.Run(void() =>
        {
            _vm.IsConnected = true;
            while (true)
            {
                if (!inputReceiver.IsRunning) break; // D_vm.IsConnected?
                if (!inputReceiver.WaitHandle.WaitOne(1000)) continue;
                if (!inputReceiver.TryRead(inputReportBuffer, 0, out var report)) continue;
                if (!inputParser.TryParseReport(inputReportBuffer, 0, report)) continue;

                ParseHidData(inputParser);
            }
            hidStream.Dispose();
            _vm.IsConnected = false;
        });
    }

    private const int RotaryPushOffset = 0;
    private const int Button300Offset = 5;
    private const int Button200Offset = 10;
    private const int Button100Offset = 15;
    private const int FaderOffset = 20;
    private const int RotaryRotOffset = 25;
    
    private void ParseHidData(DeviceItemInputParser parser)
    {
        if (!parser.HasChanged) return;
        
        // Print out values
        if (DebugHidValues)
        {
            for (int valueIndex = 0; valueIndex < parser.ValueCount; valueIndex++)
            {
                var dataValue = parser.GetValue(valueIndex);
                Console.WriteLine("{0}: {1}", (Usage)dataValue.Usages.FirstOrDefault(), dataValue.GetLogicalValue());
            }
        }

        _vm.Rotary1Push = parser.GetValue(RotaryPushOffset + 0).GetLogicalValue() == 1;
        _vm.Rotary2Push = parser.GetValue(RotaryPushOffset + 1).GetLogicalValue() == 1;
        _vm.Rotary3Push = parser.GetValue(RotaryPushOffset + 2).GetLogicalValue() == 1;
        _vm.Rotary4Push = parser.GetValue(RotaryPushOffset + 3).GetLogicalValue() == 1;
        _vm.Rotary5Push = parser.GetValue(RotaryPushOffset + 4).GetLogicalValue() == 1;
        
        _vm.Button301Push = parser.GetValue(Button300Offset + 0).GetLogicalValue() == 1;
        _vm.Button302Push = parser.GetValue(Button300Offset + 1).GetLogicalValue() == 1;
        _vm.Button303Push = parser.GetValue(Button300Offset + 2).GetLogicalValue() == 1;
        _vm.Button304Push = parser.GetValue(Button300Offset + 3).GetLogicalValue() == 1;
        _vm.Button305Push = parser.GetValue(Button300Offset + 4).GetLogicalValue() == 1;
        
        _vm.Button201Push = parser.GetValue(Button200Offset + 0).GetLogicalValue() == 1;
        _vm.Button202Push = parser.GetValue(Button200Offset + 1).GetLogicalValue() == 1;
        _vm.Button203Push = parser.GetValue(Button200Offset + 2).GetLogicalValue() == 1;
        _vm.Button204Push = parser.GetValue(Button200Offset + 3).GetLogicalValue() == 1;
        _vm.Button205Push = parser.GetValue(Button200Offset + 4).GetLogicalValue() == 1;
        
        _vm.Button101Push = parser.GetValue(Button100Offset + 0).GetLogicalValue() == 1;
        _vm.Button102Push = parser.GetValue(Button100Offset + 1).GetLogicalValue() == 1;
        _vm.Button103Push = parser.GetValue(Button100Offset + 2).GetLogicalValue() == 1;
        _vm.Button104Push = parser.GetValue(Button100Offset + 3).GetLogicalValue() == 1;
        _vm.Button105Push = parser.GetValue(Button100Offset + 4).GetLogicalValue() == 1;
        
        _vm.Fader1Cur = parser.GetValue(FaderOffset + 0).GetLogicalValue() / (double)255;
        _vm.Fader2Cur = parser.GetValue(FaderOffset + 1).GetLogicalValue() / (double)255;
        _vm.Fader3Cur = parser.GetValue(FaderOffset + 2).GetLogicalValue() / (double)255;
        _vm.Fader4Cur = parser.GetValue(FaderOffset + 3).GetLogicalValue() / (double)255;
        _vm.Fader5Cur = parser.GetValue(FaderOffset + 4).GetLogicalValue() / (double)255;
        
        _vm.Rotary1Rot += parser.GetValue(RotaryRotOffset + 0).GetLogicalValue() - 1;
        _vm.Rotary2Rot += parser.GetValue(RotaryRotOffset + 1).GetLogicalValue() - 1;
        _vm.Rotary3Rot += parser.GetValue(RotaryRotOffset + 2).GetLogicalValue() - 1;
        _vm.Rotary4Rot += parser.GetValue(RotaryRotOffset + 3).GetLogicalValue() - 1;
        _vm.Rotary5Rot += parser.GetValue(RotaryRotOffset + 4).GetLogicalValue() - 1;
    }
}
