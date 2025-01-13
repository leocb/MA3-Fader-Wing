#ifndef ARDUINO_USB_MODE
#error This ESP32 SoC has no Native USB interface

#elif ARDUINO_USB_MODE == 1
#warning This sketch should be used when USB is in OTG mode
void setup() {}
void loop() {}

#else
#include "USB.h"
#include "USBHID.h"

// Global Variables
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



// DEFINES
#define BTNS_CNT 20 // 1 bit for each button = 3 Bytes minimum
#define FADR_CNT 5  // 1 byte for each = 5 Bytes
#define ENCO_CNT 5  // 1 byte for each = 5 Bytes
#define HID_DATA_SIZE 3 + 5 + 5 // change based on above byte count
#define HID_BTNS_OFFSET 0 // always 0, unless you change the data structure
#define HID_FADR_OFFSET 3 // how many button bytes
#define HID_ENCO_OFFSET 8 // how many button + fader bytes

#define HW_BTN_PIN 0 // The soldered EPS32 S2 hardware `0` button pin

// Button matrix
#define ROWS 4
#define COLS 5
static const uint8_t rows_pins[] = {
  
};

// HID report descriptor, change marked lines if you change any of the define above
static const uint8_t report_descriptor[] = {
  // Ident as a joystick
  0x05, 0x01,  // Usage Page (Generic Desktop Ctrls)
  0x09, 0x04,  // Usage (Joystick)
  0xA1, 0x01,  // Collection (Application)
  0xa1, 0x00,  //   Collection (Physical)
  // 20 bits for buttons - binary values
  0x05, 0x09,  //   USAGE_PAGE (Button)
  0x19, 0x01,  //     USAGE_MINIMUM (Button 1)
  0x29, BTNS_CNT,  // USAGE_MAXIMUM (Button BTNS_CNT)
  0x15, 0x00,  //     LOGICAL_MINIMUM (0) 
  0x25, 0x01,  //     LOGICAL_MAXIMUM (1) 
  0x95, BTNS_CNT,  // REPORT_COUNT (BTNS_CNT) 
  0x75, 0x01,  //     REPORT_SIZE (1) 
  0x81, 0x02,  //     INPUT (Data,Var,Abs)
  // 4 bit padding <--------------------------------------------- CHANGE THIS
  // This section should pad the remaining bits from the command above to complete a full byte
  // 3 bytes = 24bits, we used 20 for the buttons, so 4 bits remain to a full byte
  0x95, 0x04,  //     REPORT_COUNT (4)  <<<<< Change this count
  0x75, 0x01,  //     REPORT_SIZE (1)
  0x81, 0x01,  //     INPUT (Const,Array,Abs)
  // 5x Faders - reported as X Y Z rX rY - absolute values <----- CHANGE THIS
  0x05, 0x01,  //     Usage Page (Generic Desktop Ctrls)
  0x09, 0x36,  //     Usage (Slider) <<<<<<<<<<<<< Add or remove usages as necessary, 0x30 to 0x36 are ok, 
  0x09, 0x36,  //     Usage (Slider) <<<<<<<<<<<<< if you need more than 7 faders, repeating 0x36 is ok.
  0x09, 0x36,  //     Usage (Slider)
  0x09, 0x36,  //     Usage (Slider)
  0x09, 0x36,  //     Usage (Slider)
  0x15, 0x00,  //     Logical Minimum (0)
  0x26, 0xff, 0x00,// Logical Maximum (255)
  0x75, 0x08,  //     Report Size (8 bytes)
  0x95, FADR_CNT,  // Report Count (FADR_CNT)
  0x81, 0x02,  //     Input (Data,Var,Abs,No Wrap,Linear,Preferred State,No Null Position)
  // 5x Rotary encoders (dials) - relative values <-------------- CHANGE THIS
  0x09, 0x37,  //     USAGE (Dial) <<<<<<<<<<<<< Add or remove usages as necessary, repeating 0x37 is ok.
  0x09, 0x37,  //     USAGE (Dial)
  0x09, 0x37,  //     USAGE (Dial)
  0x09, 0x37,  //     USAGE (Dial)
  0x09, 0x37,  //     USAGE (Dial)
  0x15, 0x00,  //     Logical Minimum (0)
  0x25, 0x02,  //     Logical Maximum (2)
  0x75, 0x08,  //     REPORT_SIZE (8) 
  0x95, ENCO_CNT,  // REPORT_COUNT (ENCO_CNT) 
  0x81, 0x06,  //     INPUT (Data,Var,Rel) 
  0xC0,        //   End Collection
  0xC0,        // End Collection
};

// Custom HID device class
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
    return HID.SendReport(0, value, HID_DATA_SIZE);
  }
};


CustomHIDDevice Device;

uint8_t data[HID_DATA_SIZE];

const int buttonPin = 0;
int previousButtonState = HIGH;

void setup() {
  Serial.begin(115200);
  Serial.setDebugOutput(true);
  pinMode(buttonPin, INPUT_PULLUP);
  Device.begin();
  USB.begin();
}

void loop() {
  // Step 0: Wait for HID to be ready
  while (!HID.ready()) {
    continue;
  }

  // Step 1: Read Button matrix state

  int buttonState = digitalRead(buttonPin);
  if (buttonState != previousButtonState) {
    previousButtonState = buttonState;
    if (buttonState == LOW) {
      SendRandomData();
    } else {
      //Serial.println("Button Released");
    }
    delay(100);
  }
}



void SendRandomData(){
  Serial.println("Button Pressed");
  // Buttons (first 20 bits)
  data[0] = random(256);
  data[1] = random(256);
  data[2] = random(256);
  // faders
  data[3] = random(256);
  data[4] = random(256);
  data[5] = random(256);
  data[6] = random(256);
  data[7] = random(256);
  // rotary
  data[8] = random(3);
  data[9] = random(3);
  data[10] = random(3);
  data[11] = random(3);
  data[12] = random(3);
  Device.send(data);  
}




#endif /* ARDUINO_USB_MODE */
