# Default App

![](.\Graphics\DefaultApp.png)

**Version 1.2** | System Information Viewer for Windows 11

A WinUI 3 desktop application that displays comprehensive system information in an easy-to-read card-based layout. Built with .NET 8 and packaged as MSIX.

Had a cdoe review and fixed some issues.

## Features

### Operating System Information
- Friendly OS Name (Windows 11/10) with Edition
- Machine Name with copy button
- Version and Build Number
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
- **Theme Support**: System Default, Light, Dark, Cyberpunk, High Contrast Dark, High Contrast Light
- **Copy to Clipboard**: Quick copy buttons for key fields (Machine Name, Version, IP, MAC, etc.)
- **Refresh**: Single button to refresh all system information
- **Window Centering**: Application window centers on primary display at launch
- **Localization Ready**: Built with .resw resource files

## Requirements

- Windows 11 (Build 22000 or later)
- x64 or ARM64 architecture

### Why Full Trust?

This application requires the `runFullTrust` capability because it accesses low-level system information not available through standard WinRT APIs:

- **P/Invoke to native DLLs** - Calls `slc.dll` for Windows activation status and `kernel32.dll` for memory and firmware information
- **HKLM Registry access** - Reads CPU details from `HKLM\HARDWARE\DESCRIPTION\System\CentralProcessor` and Windows edition from `HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion`
- **Firmware table access** - Retrieves SMBIOS data for BIOS manufacturer, version, and TPM information via `GetSystemFirmwareTable`

## Build

```bash
dotnet restore
dotnet build -c Release
dotnet publish -c Release -r win-x64
```

## What's New in 1.2

- **Improved OS Display**: Shows friendly OS name (Windows 11/10) with edition (Pro, Enterprise, etc.)
- **Machine Name**: Added to OS card with copy button
- **Window Centering**: App now centers on primary display at launch
- **Faster Startup**: Removed splash screen for instant launch
- **Theme Fixes**: Resolved theme dropdown issues
- **Code Quality**: Comprehensive unit test suite (136 tests)

## License

Proprietary - Big Hat Group Inc.
