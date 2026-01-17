## Why

The code review identified 5 medium-priority issues that affect user experience and code cleanliness:
1. Hardcoded 5-second splash screen delay is excessive
2. Unused `ApplyCustomTheme()` and `RemoveCustomTheme()` methods (dead code)
3. Debug.WriteLine statements left in production code
4. `GetTotalRamFromMemoryManager()` always returns 0 (dead code)
5. Magic numbers for ping button colors should be constants

## What Changes

- Reduce splash screen delay to 2 seconds (or make it configurable)
- Remove unused theme methods from `ThemeService`
- Remove or convert Debug.WriteLine statements to proper logging
- Remove `GetTotalRamFromMemoryManager()` method since it's non-functional
- Extract ping button colors to named constants

## Impact

- Affected code:
  - `DefaultApp/App.xaml.cs` (line 56)
  - `DefaultApp/Services/ThemeService.cs` (lines 157-206)
  - `DefaultApp/Services/HardwareInfoService.cs` (lines 291-305)
  - `DefaultApp/MainWindow.xaml.cs`
  - `DefaultApp/ViewModels/MainViewModel.cs` (lines 411-416, 482-487, 546-551)
- No breaking changes
- Improves startup time and code cleanliness
