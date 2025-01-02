# Code instructions

The code is divided in 2 parts: the source code for the embedded controller (ESP32 S2 mini) and the code of the windows app that interfaces with MA3 (.net)

## ESP32 S2 mini

This part of the project is based on the ESP32 S2 mini micro controller.
The code is done via Arduino IDE

Quick How-to:

1. Install ESP32 boards by espressif
2. Select `ESP32S2 Dev Module` board
3. Reset the board into bootloader mode (Hold the `0` button and tap the `reset` button, release the `0` button)
4. Select the ESP comm port
5. Upload

### Useful USB HID links:

#### HID Docs

[Usage Tables](https://usb.org/sites/default/files/hut1_5.pdf)
[HID Implementation](https://usb.org/sites/default/files/hid1_11.pdf)
[HID Class for C# on Windows 10+](https://learn.microsoft.com/en-us/uwp/api/windows.gaming.input.custom.hidgamecontrollerprovider?view=winrt-26100)
[ESP32 Custom HID Youtube tutorial](https://www.youtube.com/watch?v=_FQtUf3RdEA)

#### HID Tools

[View all USB devices in a hierarchal tree, with info](https://www.uwe-sieber.de/usbtreeview_e.html)
[Dump HID Report Descriptor from all devices](https://github.com/todbot/win-hid-dump)
[Parse the data dumped from the tool above](https://eleccelerator.com/usbdescreqparser/)
[View HID descriptor and realtime data on the browser](https://nondebug.github.io/webhid-explorer/)

## Windows App

The windows app is written in .net

I (will) Use the JetBrains Ridder IDE to develop it (Free for non-commercial). Try it out yourself [here](https://www.jetbrains.com/rider/)
