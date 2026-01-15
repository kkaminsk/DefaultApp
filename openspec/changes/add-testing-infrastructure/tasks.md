## 1. Test Project Setup

- [ ] 1.1 Configure DefaultApp.Tests with xUnit
- [ ] 1.2 Add Microsoft.NET.Test.Sdk package
- [ ] 1.3 Add xunit and xunit.runner.visualstudio packages
- [ ] 1.4 Add Moq for mocking dependencies
- [ ] 1.5 Configure test coverage with coverlet

## 2. SystemInfoService Tests

- [ ] 2.1 Create SystemInfoServiceTests class
- [ ] 2.2 Test GetOsName() returns valid value
- [ ] 2.3 Test GetOsVersion() returns valid version
- [ ] 2.4 Test GetEdition() handles Registry success
- [ ] 2.5 Test GetEdition() returns "Unavailable" on Registry failure
- [ ] 2.6 Test GetSystemLocale() returns valid locale

## 3. HardwareInfoService Tests

- [ ] 3.1 Create HardwareInfoServiceTests class
- [ ] 3.2 Test GetProcessorArchitecture() returns valid architecture
- [ ] 3.3 Test GetMachineName() returns non-empty string
- [ ] 3.4 Test GetTotalRam() formats correctly (XX.X GB)
- [ ] 3.5 Test GetIsRunningUnderEmulation() logic
- [ ] 3.6 Test GetCpuModels() handles multi-processor systems

## 4. ActivationService Tests

- [ ] 4.1 Create ActivationServiceTests class
- [ ] 4.2 Test async operation returns "Checking..." initially
- [ ] 4.3 Test result caching after first call
- [ ] 4.4 Test each ActivationStatus enum value mapping
- [ ] 4.5 Mock P/Invoke calls for testability

## 5. MainViewModel Tests

- [ ] 5.1 Create MainViewModelTests class
- [ ] 5.2 Test LoadDataAsync() populates all properties
- [ ] 5.3 Test RefreshCommand triggers data reload
- [ ] 5.4 Test IsRefreshing property state changes
- [ ] 5.5 Test service type selection changes

## 6. ServiceController Tests

- [ ] 6.1 Create ServiceControllerTests class
- [ ] 6.2 Test service state transitions
- [ ] 6.3 Test uptime tracking
- [ ] 6.4 Test auto-stop on service switch
- [ ] 6.5 Test error handling scenarios

## 7. UI Automation Tests

- [ ] 7.1 Add Microsoft.Windows.Apps.Test package
- [ ] 7.2 Create UI test project or folder
- [ ] 7.3 Test main window launches correctly
- [ ] 7.4 Test all three cards are visible
- [ ] 7.5 Test theme selector changes theme
- [ ] 7.6 Test refresh button functionality
- [ ] 7.7 Test service type selection

## 8. Accessibility Tests

- [ ] 8.1 Create accessibility test helpers
- [ ] 8.2 Test all interactive controls have automation names
- [ ] 8.3 Test keyboard navigation order
- [ ] 8.4 Test tab key traverses all controls
- [ ] 8.5 Test high contrast mode rendering

## 9. Test Coverage

- [ ] 9.1 Configure coverlet for coverage collection
- [ ] 9.2 Set minimum coverage threshold to 80% for Services/
- [ ] 9.3 Generate coverage reports in CI
- [ ] 9.4 Add coverage badge to README

## 10. Validation

- [ ] 10.1 All unit tests pass
- [ ] 10.2 UI automation tests pass
- [ ] 10.3 Accessibility tests pass
- [ ] 10.4 Coverage meets 80% target for service layer
