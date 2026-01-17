# Proposal: Improve System Info Display

## Why

The current system information display has usability issues: the OS name shows a technical "Win32NT" identifier instead of a friendly name, the activation status check fails in MSIX context, the Machine Name is illogically placed in the Hardware card instead of with OS info, and users cannot easily copy common fields like machine name and version.

## Summary

Enhance the system information display with friendlier OS naming, improved activation status detection, reorganized field layout, and copy-to-clipboard functionality for key fields.

## Motivation

The current implementation has several usability issues:
1. **OS Name** displays as "Win32NT" which is not user-friendly
2. **Activation Status** uses `SLIsGenuineLocal` P/Invoke which returns "Unavailable" in MSIX context
3. **Machine Name** is in the Hardware card but logically belongs with OS information
4. **No copy functionality** for fields users commonly need to share (Machine Name, Version)

## Changes

### 1. Friendly OS Name
- Replace "Win32NT" with "Windows 11 Enterprise" format
- Combine Windows version detection (build number) with Edition from Registry
- Build 22000+ = Windows 11, Build 10240-21999 = Windows 10

### 2. Registry-based Activation Status
- Replace `SLIsGenuineLocal` P/Invoke with Registry-based detection
- Read from `HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform`
- More reliable in MSIX packaged context

### 3. Move Machine Name Field
- Move Machine Name from "Device & Hardware" card to "Operating System" card
- Position it directly under the OS Name field for logical grouping

### 4. Copy to Clipboard Icons
- Add copy button beside Machine Name field
- Add copy button beside Version field
- Use Segoe Fluent Icons copy glyph (&#xE8C8;)
- Show tooltip on hover, brief confirmation on copy

## Impact

- **SystemInfoService**: Add friendly OS name generation method
- **ActivationService**: Replace P/Invoke with Registry lookup
- **MainPage.xaml**: Reorganize fields, add copy buttons
- **MainViewModel**: Add copy commands, move MachineName property display
- **Resources.resw**: Add copy button accessibility strings

## Dependencies

None - self-contained UI/service changes.
