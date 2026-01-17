## 1. Add SystemInfoService Tests

- [ ] 1.1 Create `SystemInfoServiceTests.cs`
- [ ] 1.2 Test `GetOsName()` returns valid platform
- [ ] 1.3 Test `GetFriendlyOsName()` returns "Windows 10" or "Windows 11"
- [ ] 1.4 Test `GetOsVersion()` returns valid version string
- [ ] 1.5 Test `GetBuildNumber()` returns numeric string
- [ ] 1.6 Test `GetEdition()` returns valid edition or "Unavailable"
- [ ] 1.7 Test `GetSystemLocale()` returns valid culture name
- [ ] 1.8 Test `MapEditionToDisplayName()` maps known editions correctly

## 2. Add HardwareInfoService Tests

- [ ] 2.1 Create `HardwareInfoServiceTests.cs`
- [ ] 2.2 Test `GetProcessorArchitecture()` returns valid architecture
- [ ] 2.3 Test `GetOsArchitecture()` returns valid architecture
- [ ] 2.4 Test `GetMachineName()` returns non-empty string
- [ ] 2.5 Test `GetProcessorCount()` returns positive number
- [ ] 2.6 Test `GetCpuModels()` returns non-empty list
- [ ] 2.7 Test `GetTotalRam()` returns formatted string with "GB"
- [ ] 2.8 Test `GetIsRunningUnderEmulation()` logic with various inputs
- [ ] 2.9 Test `FormatBytesAsGb()` (if made internal/public for testing)

## 3. Add ActivationService Tests

- [ ] 3.1 Create `ActivationServiceTests.cs`
- [ ] 3.2 Test `GetActivationStatusDisplay()` returns correct strings for all enum values
- [ ] 3.3 Test `ClearCache()` resets cached status
- [ ] 3.4 Test caching behavior (second call returns cached value)

## 4. Add NetworkInfoService Tests

- [ ] 4.1 Create `NetworkInfoServiceTests.cs`
- [ ] 4.2 Test `GetNetworkInfo()` returns valid NetworkInfo object
- [ ] 4.3 Test `PingAsync()` with invalid address returns false
- [ ] 4.4 Test `PingAsync()` with "Unavailable" returns false
- [ ] 4.5 Test `PingAsync()` with empty string returns false

## 5. Add BiosInfoService Tests

- [ ] 5.1 Create `BiosInfoServiceTests.cs`
- [ ] 5.2 Test `GetBiosInfo()` returns valid BiosInfo object
- [ ] 5.3 Test `GetSecureBootStatus()` returns boolean
- [ ] 5.4 Test `GetSmbiosVersion()` returns version string or "Unavailable"

## 6. Add TpmInfoService Tests

- [ ] 6.1 Create `TpmInfoServiceTests.cs`
- [ ] 6.2 Test `GetTpmInfo()` returns valid TpmInfo object
- [ ] 6.3 Test `ConvertManufacturerIdToName()` with known manufacturer IDs

## 7. Add ThemeService Tests

- [ ] 7.1 Create `ThemeServiceTests.cs`
- [ ] 7.2 Test `FromComboBoxIndex()` maps indices correctly
- [ ] 7.3 Test `ToComboBoxIndex()` maps themes correctly
- [ ] 7.4 Test round-trip conversion (index -> theme -> index)

## 8. Add MainViewModel Tests

- [ ] 8.1 Create `MainViewModelTests.cs`
- [ ] 8.2 Test `FormatCpuModels()` with empty list
- [ ] 8.3 Test `FormatCpuModels()` with single CPU
- [ ] 8.4 Test `FormatCpuModels()` with multiple CPUs
- [ ] 8.5 Test `CopyToClipboard()` with valid field names
- [ ] 8.6 Test `CopyToClipboard()` with unknown field name
- [ ] 8.7 Test `CanRefresh()` returns false when refreshing

## 9. Validation

- [ ] 9.1 Run all tests and verify they pass
- [ ] 9.2 Generate code coverage report
- [ ] 9.3 Verify service layer coverage meets 80% target
- [ ] 9.4 Document any areas that cannot be easily tested
