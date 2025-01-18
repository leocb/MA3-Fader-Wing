
#include <Adafruit_TinyUSB.h>
#include "esp_mac.h"

// Global Variables

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
#define USB_SERIAL_NUMBER "54317092"
// 93008922
// 41851570
// 31921269

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

// Custom HID device class// HID device
Adafruit_USBD_HID usb_hid;

uint8_t data[HID_DATA_SIZE];

const int buttonPin = 0;
int previousButtonState = HIGH;

void setup() {
  setupUsb();
  pinMode(buttonPin, INPUT_PULLUP);
}

void loop() {
  // Manual call tud_task since it isn't called by Core's background
  #ifdef TINYUSB_NEED_POLLING_TASK
  TinyUSBDevice.task();
  #endif

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
  // Send report
  usb_hid.sendReport(0, data, sizeof(data));
}






//////////////////////////////////////////////////////
/// ------------------ USB HID ------------------- ///
//////////////////////////////////////////////////////


// HID report descriptor, change marked lines if you change any of the define above
uint8_t const desc_hid_report[] = {
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

void setupUsb(){
  // Manual begin() is required on core without built-in support e.g. mbed rp2040
  if (!TinyUSBDevice.isInitialized()) {
    TinyUSBDevice.begin(0);
  }

  // Initialize HID
  usb_hid.enableOutEndpoint(true);
  usb_hid.setPollInterval(2); // 2ms polling interval
  usb_hid.setReportDescriptor(desc_hid_report, sizeof(desc_hid_report));
  TinyUSBDevice.setManufacturerDescriptor("github.com/leocb");
  TinyUSBDevice.setProductDescriptor("DIY MA3 FaderWing");
  TinyUSBDevice.setSerialDescriptor(USB_SERIAL_NUMBER);
  usb_hid.begin();
  
  // If already enumerated, additional class driverr begin() e.g msc, hid, midi won't take effect until re-enumeration
  if (TinyUSBDevice.mounted()) {
    TinyUSBDevice.detach();
    delay(10);
    TinyUSBDevice.attach();
  }
}
