# Proposal: Add TPM Information Card

## Change ID
`add-tpm-info-card`

## Summary

Add a new "TPM Information" card at the bottom of the main page displaying TPM (Trusted Platform Module) details with copy-to-clipboard functionality for each field.

## Motivation

TPM information is important for IT professionals and developers to verify security hardware configuration. This extends the system information displayed by the application.

## Scope

### In Scope
- New TPM Information card at bottom of page
- Four TPM properties with copy buttons:
  - SpecVersion
  - ManufacturerId
  - ManufacturerVersion
  - PhysicalPresenceVersionInfo
- TpmInfoService to retrieve TPM data via Registry

### Out of Scope
- TPM health status or diagnostics
- TPM configuration/management

## Solution Overview

1. Create `TpmInfoService` to read TPM data from Registry
2. Create `TpmInfo` model class
3. Add TPM properties to `MainViewModel`
4. Add TPM Information card to `MainPage.xaml`
5. Update `CopyToClipboard` handler for new fields

## Data Source

TPM information is available in the Registry at:
`HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TPM\WMI`

| Property | Registry Value |
|----------|----------------|
| SpecVersion | `SpecVersion` |
| ManufacturerId | `ManufacturerId` |
| ManufacturerVersion | `ManufacturerVersion` |
| PhysicalPresenceVersionInfo | `PhysicalPresenceVersionInfo` |

## Files to Create/Modify

| File | Action |
|------|--------|
| `Models/TpmInfo.cs` | Create - TPM data model |
| `Services/TpmInfoService.cs` | Create - TPM Registry reader |
| `ViewModels/MainViewModel.cs` | Modify - Add TPM properties |
| `Views/MainPage.xaml` | Modify - Add TPM card |
