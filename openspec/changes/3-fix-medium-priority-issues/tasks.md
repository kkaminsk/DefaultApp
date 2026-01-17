## 1. Reduce Splash Screen Delay

- [x] 1.1 Change `Task.Delay(5000)` to `Task.Delay(2000)` in `App.xaml.cs`
- [x] 1.2 Optionally, consider making the delay configurable or splash-until-ready

## 2. Remove Unused Theme Methods

- [x] 2.1 Remove `ApplyCustomTheme()` method from `ThemeService`
- [x] 2.2 Remove `RemoveCustomTheme()` method from `ThemeService`
- [x] 2.3 Verify no remaining references to removed methods

## 3. Clean Up Debug.WriteLine Statements

- [x] 3.1 Remove Debug.WriteLine statements from `ThemeService.cs`
- [x] 3.2 Remove Debug.WriteLine statements from `MainWindow.xaml.cs`
- [x] 3.3 Replace any important debug output with proper ILogger calls

## 4. Remove Dead RAM Detection Code

- [x] 4.1 Remove `GetTotalRamFromMemoryManager()` method from `HardwareInfoService`
- [x] 4.2 Update `GetTotalRam()` to call P/Invoke directly without fallback logic
- [x] 4.3 Simplify the RAM retrieval flow

## 5. Extract Ping Color Constants

- [x] 5.1 Define constants for ping result colors in `MainViewModel` (Green, Yellow, Red)
- [x] 5.2 Update `ExecutePingAsync` to use named constants
- [x] 5.3 Update `ExecutePingDnsAsync` to use named constants
- [x] 5.4 Update `ExecutePingGoogleDnsAsync` to use named constants
- [x] 5.5 Consider moving colors to a shared location or theme resources

## 6. Validation

- [x] 6.1 Build and verify no compilation errors
- [x] 6.2 Verify splash screen displays for ~2 seconds
- [x] 6.3 Test theme switching still works
- [x] 6.4 Verify RAM is still displayed correctly
- [x] 6.5 Verify ping button colors still work correctly
