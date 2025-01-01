# DIY GrandMA3 Fader Wing

An open source alternative hardware for the MA3 Fader wing. Intended for budget onPC setups.

This project should be:

- All electronic parts easily obtainable and be affordable
- 3D print everything
- easy to assemble

## Disclaimers ⚠️

> - **This project is a work in progress and is NOT ready yet!**
> - The project had a drastic change in direction, [read below](#explaining-the-project-changes) for more info.
> - This is a simple control surface that will send MIDI and/or OSC messages to the MA3 onPC software, it will **NOT** enable parameters on the MA3. If you need to enable parameters, the cheapest way to do it is by purchasing the MA3 2port Node with 4k parameters.
> - This project uses the GPL-3.0 license, which basically means that you can sell this if you want to, but any changes *must* be kept open source!

## Contributing 🤝

### Project contributions

- Feel free to open a PR with new features or bug fixes
- Use [KiCad 8](https://www.kicad.org/download/) or newer to open the PCB design files

### Financial support

Support this and more projects by donating:

[![Paypal](https://user-images.githubusercontent.com/8310271/225498353-9d0a672d-ed45-4fed-9838-11d71ee49c28.png)](https://www.paypal.com/donate/?hosted_button_id=683D7S6KLX7EA)

## How to make your own 🚀

Coming soon™

I'm currently working on a prototype that should be ready in the near future.

## Parts List (NOT FINAL!)

- 15x Cherry MX compatible keyboard switches (and key caps)
- 5x Rotary knob with integrated push buttons (and knob caps)
- 5x Linear Fader (and fader caps)
- 20x 1n4148 Diode
- 15x 100nf Ceramic Capacitors
- ESP32 S2 mini
- 3D Printed enclosure and modules
- Lots of 30awg wire
- Perforated prototype PCB board

## PCB / Electronic Design ✏️

Coming soon™

## The code

Follow the code specific instructions [here](Code/README.md).

## Explaining the project changes

The initial intent behind this project was to make an affordable, DIY alternative for the MA3 command wing, having a custom PCB goes against the "affordable" part of it.

So, I've decided to remove the custom PCB entirely from this project and go with off the shelf parts. The previous iteration of this project is available on another git branch named `Legacy` in this same repository.

While the KiCad files in there are, in theory, ready for production, it has not been tested and the code for it has never been implemented. Feel free to use it however you like.
