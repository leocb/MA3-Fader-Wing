#ifndef ARDUINO_USB_MODE
#error This ESP32 SoC has no Native USB interface
#elif ARDUINO_USB_MODE == 1
#warning This sketch should be used when USB is in OTG mode
void setup() {}
void loop() {}
#else
#include "USB.h"
#include "USBHID.h"
USBHID HID;


// MA3 Fader wing what:

// Buttons
// 4 Rows (3 buttons, 1 encoders), set as Output, set to HIGH when processing the row
// 5 Cols, Input (PullDown), '1' if button was pressed, 0 otherwise
// This creates a matrix of 20 buttons that should be sent as part of a Joystick HID

// Encoders
// 10 additional "buttons" act as the 5x encoders rotating Left/Right, by pulsing the button
// (is there a better way to do this?)

// Faders
// 5 sliders for the faders, value goes from 0 -> 255 

// Meta
// Internal copy of current controler state and compare with processed state,
// send update if anything changed, else quick jump back to polling




/* Original descriptor for reference
static const uint8_t report_descriptor[] = {
  // 8 axis
  0x05, 0x01,  // Usage Page (Generic Desktop Ctrls)
  0x09, 0x04,  // Usage (Joystick)
  0xa1, 0x01,  // Collection (Application)
  0xa1, 0x00,  //   Collection (Physical)
  0x05, 0x01,  //   Usage Page (Generic Desktop Ctrls)
  0x09, 0x30,  //     Usage (X)
  0x09, 0x31,  //     Usage (Y)
  0x09, 0x32,  //     Usage (Z)
  0x09, 0x33,  //     Usage (Rx)
  0x09, 0x34,  //     Usage (Ry)
  0x09, 0x35,  //     Usage (Rz)
  0x09, 0x36,  //     Usage (Slider)
  0x09, 0x36,  //     Usage (Slider)
  0x15, 0x81,  //     Logical Minimum (-127)
  0x25, 0x7f,  //     Logical Maximum (127)
  0x75, 0x08,  //     Report Size (8)
  0x95, 0x08,  //     Report Count (8)
  0x81, 0x02,  //     Input (Data,Var,Abs,No Wrap,Linear,Preferred State,No Null Position)
  0xC0,        //   End Collection
  0xC0,        // End Collection
};
*/

static const uint8_t report_descriptor[] = {
  // Ident as a joystick
  0x05, 0x01,  // Usage Page (Generic Desktop Ctrls)
  0x09, 0x04,  // Usage (Joystick)
  0xA1, 0x01,  // Collection (Application)
  0xa1, 0x00,  //   Collection (Physical)
  // 20 bits for buttons - binary values
  0x05, 0x09,  //   USAGE_PAGE (Button)
  0x19, 0x01,  //     USAGE_MINIMUM (Button 1)
  0x29, 0x14,  //     USAGE_MAXIMUM (Button 20)
  0x15, 0x00,  //     LOGICAL_MINIMUM (0) 
  0x25, 0x01,  //     LOGICAL_MAXIMUM (1) 
  0x95, 0x14,  //     REPORT_COUNT (20) 
  0x75, 0x01,  //     REPORT_SIZE (1) 
  0x81, 0x02,  //     INPUT (Data,Var,Abs)
  // 4 bit padding
  0x95, 0x04,  //     REPORT_COUNT (4)
  0x75, 0x01,  //     REPORT_SIZE (1)
  0x81, 0x01,  //     INPUT (Const,Array,Abs)
  // 5x Faders - reported as X Y Z rX rY - absolute values
  0x05, 0x01,  //     Usage Page (Generic Desktop Ctrls)
  0x09, 0x30,  //     Usage (X)
  0x09, 0x31,  //     Usage (Y)
  0x09, 0x32,  //     Usage (Z)
  0x09, 0x33,  //     Usage (rX)
  0x09, 0x34,  //     Usage (rY)
  0x15, 0x81,  //     Logical Minimum (-127)
  0x25, 0x7f,  //     Logical Maximum (127)
  0x75, 0x08,  //     Report Size (8 bytes)
  0x95, 0x05,  //     Report Count (5 = 5x sliders (X/Y/S/S/S))
  0x81, 0x02,  //     Input (Data,Var,Abs,No Wrap,Linear,Preferred State,No Null Position)
  // 5x Rotary encoders (dials) - relative values
  0x09, 0x37,  //     USAGE (Dial)
  0x09, 0x37,  //     USAGE (Dial)
  0x09, 0x37,  //     USAGE (Dial)
  0x09, 0x37,  //     USAGE (Dial)
  0x09, 0x37,  //     USAGE (Dial)
  0x15, 0x81,  //     LOGICAL_MINIMUM (-127) 
  0x25, 0x7F,  //     LOGICAL_MAXIMUM (127) 
  0x75, 0x08,  //     REPORT_SIZE (8) 
  0x95, 0x05,  //     REPORT_COUNT (5) 
  0x81, 0x06,  //     INPUT (Data,Var,Rel) 
  0xC0,        //   End Collection
  0xC0,        // End Collection
};

#define __DATA_SIZE 3+5+5 // 3 bytes for buttons, 5 bytes for faders, 5 bytes for encoders

class CustomHIDDevice : public USBHIDDevice {
public:
  CustomHIDDevice(void) {
    static bool initialized = false;
    if (!initialized) {
      initialized = true;
      HID.addDevice(this, sizeof(report_descriptor));
    }
  }

  void begin(void) {
    HID.begin();
  }

  uint16_t _onGetDescriptor(uint8_t *buffer) {
    memcpy(buffer, report_descriptor, sizeof(report_descriptor));
    return sizeof(report_descriptor);
  }
  
  bool send(uint8_t *value) {
    return HID.SendReport(0, value, __DATA_SIZE);
  }
};

CustomHIDDevice Device;

const int buttonPin = 0;
int previousButtonState = HIGH;
uint8_t data[__DATA_SIZE];

void setup() {
  Serial.begin(115200);
  Serial.setDebugOutput(true);
  pinMode(buttonPin, INPUT_PULLUP);
  Device.begin();
  USB.begin();
}
int number = 0;
void loop() {
  int buttonState = digitalRead(buttonPin);
  if (HID.ready() && buttonState != previousButtonState) {
    previousButtonState = buttonState;
    if (buttonState == LOW) {
      number++;
      Serial.println("Button Pressed");
      data[0] = number;
      data[1] = number + 128;
      data[2] = random() & 0xFF;
      data[3] = random() & 0xFF;
      data[4] = random() & 0xFF;
      data[5] = random() & 0xFF;
      data[6] = random() & 0xFF;
      data[7] = random() & 0xFF;
      data[8] = number;
      data[9] = number;
      data[10] = number;
      data[11] = number;
      data[12] = number;
      Device.send(data);
    } else {
      Serial.println("Button Released");
    }
    delay(100);
  }
}
#endif /* ARDUINO_USB_MODE */
