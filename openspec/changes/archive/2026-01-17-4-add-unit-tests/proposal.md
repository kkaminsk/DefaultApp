## Why

The code review identified that test coverage is minimal. The current test file contains only infrastructure validation tests, not actual unit tests for services or ViewModels. The project documentation states a target of 80% coverage for the service layer, but current coverage is effectively 0%.

## What Changes

- Add unit tests for `SystemInfoService`
- Add unit tests for `HardwareInfoService`
- Add unit tests for `ActivationService`
- Add unit tests for `NetworkInfoService`
- Add unit tests for `BiosInfoService`
- Add unit tests for `TpmInfoService`
- Add unit tests for `ThemeService`
- Add unit tests for `MainViewModel` (key behaviors)

## Impact

- Affected code:
  - `DefaultApp.Tests/` (new test files)
- No changes to production code
- Improves code reliability and regression detection
- Moves toward 80% service layer coverage target
