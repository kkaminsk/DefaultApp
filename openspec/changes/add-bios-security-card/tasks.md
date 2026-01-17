## 1. Model Layer

- [x] 1.1 Create `BiosInfo` model with properties for all BIOS fields

## 2. Service Layer

- [x] 2.1 Create `BiosInfoService` with `GetBiosInfo()` method
- [x] 2.2 Implement Registry retrieval for BIOS Manufacturer, Name, Version, Release Date
- [x] 2.3 Implement SMBIOS Version calculation from BiosMajorRelease.BiosMinorRelease
- [x] 2.4 Implement Secure Boot status retrieval from SecureBoot registry key

## 3. ViewModel Layer

- [x] 3.1 Add BIOS properties to `MainViewModel`
- [x] 3.2 Add `BiosInfoService` instantiation and data loading
- [x] 3.3 Update `CopyToClipboardCommand` to handle new BIOS fields

## 4. UI Layer

- [x] 4.1 Add "BIOS & Security" card to MainPage.xaml with all fields
- [x] 4.2 Add copy buttons to all fields except Secure Boot Status
- [x] 4.3 Add localization strings for all labels and copy buttons

## 5. Validation

- [x] 5.1 Build and verify the new card displays correctly
- [x] 5.2 Test all copy button functionality
- [x] 5.3 Run existing tests to ensure no regressions (10 tests passed)
