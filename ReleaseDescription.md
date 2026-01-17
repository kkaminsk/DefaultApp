# Default App v1.2

A WinUI 3 desktop application that displays comprehensive system information in an easy-to-read card-based layout.

## What's New

- **Improved OS Display** - Shows friendly OS name (Windows 11/10) with edition (Pro, Enterprise, etc.)
- **Machine Name** - Added to OS card with copy button
- **Window Centering** - App now centers on primary display at launch
- **Faster Startup** - Removed splash screen for instant launch
- **Theme Fixes** - Resolved theme dropdown issues
- **Code Quality** - Comprehensive unit test suite (136 tests)

## Features

- Operating system details with activation status
- CPU, RAM, and architecture information
- BIOS and Secure Boot status
- TPM manufacturer and firmware version
- Network configuration with ping tests
- Six theme options including high contrast modes
- Copy buttons for key fields

## Requirements

- Windows 11 (Build 22000 or later)
- x64 or ARM64 architecture

## Installation

1. Download the MSIX package for your architecture
2. Double-click to install (certificate is from a trusted CA)
3. Launch "Default App" from the Start menu

## Package Details

| Property | Value |
|----------|-------|
| Publisher | Big Hat Group Inc. |
| Signing | Azure Trusted Signing |
| Framework | .NET 8 / WinUI 3 |
