## Why

Users need to identify their device model for support, documentation, and compatibility purposes. The Device Model (e.g., "Surface Pro 9", "Dell XPS 15") provides essential hardware identification.

## What Changes

- Add Device Model field to Device & Hardware card, positioned below Processor Count
- Add copy to clipboard button for Device Model
- Retrieve device model from Registry (`HKLM\HARDWARE\DESCRIPTION\System\BIOS`)
- Add `DeviceModel` property to `ArchitectureInfo` model
- Add `GetDeviceModel()` method to `HardwareInfoService`

## Impact

- Affected specs: `system-info`
- Affected code:
  - `DefaultApp/Services/HardwareInfoService.cs`
  - `DefaultApp/Models/ArchitectureInfo.cs`
  - `DefaultApp/ViewModels/MainViewModel.cs`
  - `DefaultApp/Views/MainPage.xaml`
  - `DefaultApp/Strings/en-US/Resources.resw`
