## Why

Users need to view network configuration details alongside other system information for diagnostics and troubleshooting purposes.

## What Changes

- Add a new "Network Information" card displaying IP Address, Subnet Mask, Default Gateway, DNS Server, and MAC Address
- Each field includes a copy-to-clipboard button
- Create NetworkInfoService using System.Net.NetworkInformation (no WMI)

## Impact

- Affected specs: New `network-info` capability
- Affected code:
  - `DefaultApp/Services/NetworkInfoService.cs` - New service
  - `DefaultApp/Models/NetworkInfo.cs` - New model
  - `DefaultApp/ViewModels/MainViewModel.cs` - Add network properties
  - `DefaultApp/Views/MainPage.xaml` - Add Network Information card
  - `DefaultApp/Strings/en-US/Resources.resw` - Add localization strings
