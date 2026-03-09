## Why

Users need visibility into BIOS and security settings for system diagnostics, firmware updates, and security compliance verification. A dedicated card for BIOS and security information centralizes this critical system data.

## What Changes

- Add a third card "BIOS & Security" to the main page
- Display BIOS information retrieved from Registry:
  - BIOS Manufacturer (with copy button)
  - BIOS Name (with copy button)
  - BIOS Version (with copy button)
  - BIOS Release Date (with copy button)
  - SMBIOS Version (with copy button)
  - Secure Boot Status (no copy button)
- Create `BiosInfoService` for retrieving BIOS data
- Create `BiosInfo` model for BIOS properties

## Impact

- Affected specs: `main-ui`
- Affected code:
  - New: `DefaultApp/Services/BiosInfoService.cs`
  - New: `DefaultApp/Models/BiosInfo.cs`
  - `DefaultApp/ViewModels/MainViewModel.cs`
  - `DefaultApp/Views/MainPage.xaml`
  - `DefaultApp/Strings/en-US/Resources.resw`
