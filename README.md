# Default App

**Version 1.1** | System Information Viewer for Windows 11

A WinUI 3 desktop application that displays comprehensive system information in an easy-to-read card-based layout. Built with .NET 8 and packaged as MSIX.

## Features

### Operating System Information
- OS Name, Version, and Build Number
- 64-bit OS detection
- System Locale
- Windows Activation Status
- Test Audio button for audio diagnostics

### Device & Hardware Information
- CPU Model (supports multi-processor display)
- Processor Count
- Device Model and Serial Number
- Total RAM and VRAM
- Processor and OS Architecture
- Emulation detection (for ARM devices)

### BIOS & Security Information
- BIOS Manufacturer, Name, and Version
- BIOS Release Date
- SMBIOS Version
- Secure Boot Status

### TPM Information
- TPM Manufacturer
- TPM Firmware Version

### Network Information
- IP Address and Subnet Mask
- Default Gateway with Ping test
- DNS Server with Ping test
- Google DNS (8.8.8.8) with Ping test for external connectivity verification
- MAC Address

### Additional Features
- **Theme Support**: System Default, Inverted, Dark, Cyberpunk, High Contrast Dark, High Contrast Light
- **Copy to Clipboard**: Quick copy buttons for key fields
- **Refresh**: Single button to refresh all system information
- **Localization Ready**: Built with .resw resource files

## Requirements

- Windows 11 (Build 22000 or later)
- x64 or ARM64 architecture

## Build

```bash
dotnet restore
dotnet build -c Release
dotnet publish -c Release -r win-x64
```

## License

Proprietary - Big Hat Group Inc.
