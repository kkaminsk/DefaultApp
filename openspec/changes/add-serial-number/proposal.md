## Why

Users need to identify their device serial number for warranty lookups, asset management, and support requests. The serial number should be displayed alongside the Device Model for easy access.

## What Changes

- Add Serial Number field to Device & Hardware card, positioned below Device Model
- Add copy to clipboard button for Serial Number
- Retrieve serial number from Registry (`HKLM\HARDWARE\DESCRIPTION\System\BIOS`)
- Try `SystemSerialNumber` first, fall back to `BaseBoardSerialNumber` if not available
- Add `SerialNumber` property to `ArchitectureInfo` model
- Add `GetSerialNumber()` method to `HardwareInfoService`

## Impact

- Affected specs: `system-info`
- Affected code:
  - `DefaultApp/Services/HardwareInfoService.cs`
  - `DefaultApp/Models/ArchitectureInfo.cs`
  - `DefaultApp/ViewModels/MainViewModel.cs`
  - `DefaultApp/Views/MainPage.xaml`
  - `DefaultApp/Strings/en-US/Resources.resw`
