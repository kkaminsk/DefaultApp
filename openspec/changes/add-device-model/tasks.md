## 1. Service Layer

- [x] 1.1 Add `DeviceModel` property to `ArchitectureInfo` model
- [x] 1.2 Add `GetDeviceModel()` method to `HardwareInfoService` using Registry
- [x] 1.3 Update `GetArchitectureInfo()` to populate `DeviceModel`

## 2. ViewModel Layer

- [x] 2.1 Add `DeviceModel` property to `MainViewModel`
- [x] 2.2 Update `CopyToClipboardCommand` to handle "DeviceModel" parameter

## 3. UI Layer

- [x] 3.1 Add Device Model field with copy button to MainPage.xaml (below Processor Count)
- [x] 3.2 Add localization strings for Device Model label and copy button in Resources.resw

## 4. Validation

- [x] 4.1 Build and verify the new field displays correctly
- [x] 4.2 Test copy button functionality
- [x] 4.3 Run existing tests to ensure no regressions (10 tests passed)
