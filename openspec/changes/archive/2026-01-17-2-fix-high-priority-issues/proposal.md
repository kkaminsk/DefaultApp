## Why

The code review identified 4 high-priority issues that affect code quality, maintainability, and correctness:
1. Empty catch blocks silently swallow exceptions without logging
2. Three nearly identical ping methods violate DRY principle
3. `GetBiosName()` and `GetBiosVersion()` return the same registry value (bug)
4. `ThemeService` subscribes to events but never unsubscribes (memory leak)

## What Changes

- Add exception logging to all empty catch blocks in services
- Refactor ping methods in `MainViewModel` to use a common implementation
- Fix `BiosInfoService.GetBiosName()` to return a distinct value or remove if redundant
- Implement `IDisposable` on `ThemeService` and unsubscribe from `ColorValuesChanged`

## Impact

- Affected code:
  - `DefaultApp/Services/SystemInfoService.cs`
  - `DefaultApp/Services/HardwareInfoService.cs`
  - `DefaultApp/Services/LoggingService.cs`
  - `DefaultApp/Services/BiosInfoService.cs` (lines 55-65)
  - `DefaultApp/Services/ThemeService.cs` (line 40)
  - `DefaultApp/ViewModels/MainViewModel.cs` (lines 387-559)
- No breaking changes to public API
- Improves debugging capability and code maintainability
