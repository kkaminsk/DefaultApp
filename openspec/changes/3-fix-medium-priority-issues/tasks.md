## 1. Reduce Splash Screen Delay

- [ ] 1.1 Change `Task.Delay(5000)` to `Task.Delay(2000)` in `App.xaml.cs`
- [ ] 1.2 Optionally, consider making the delay configurable or splash-until-ready

## 2. Remove Unused Theme Methods

- [ ] 2.1 Remove `ApplyCustomTheme()` method from `ThemeService`
- [ ] 2.2 Remove `RemoveCustomTheme()` method from `ThemeService`
- [ ] 2.3 Verify no remaining references to removed methods

## 3. Clean Up Debug.WriteLine Statements

- [ ] 3.1 Remove Debug.WriteLine statements from `ThemeService.cs`
- [ ] 3.2 Remove Debug.WriteLine statements from `MainWindow.xaml.cs`
- [ ] 3.3 Replace any important debug output with proper ILogger calls

## 4. Remove Dead RAM Detection Code

- [ ] 4.1 Remove `GetTotalRamFromMemoryManager()` method from `HardwareInfoService`
- [ ] 4.2 Update `GetTotalRam()` to call P/Invoke directly without fallback logic
- [ ] 4.3 Simplify the RAM retrieval flow

## 5. Extract Ping Color Constants

- [ ] 5.1 Define constants for ping result colors in `MainViewModel` (Green, Yellow, Red)
- [ ] 5.2 Update `ExecutePingAsync` to use named constants
- [ ] 5.3 Update `ExecutePingDnsAsync` to use named constants
- [ ] 5.4 Update `ExecutePingGoogleDnsAsync` to use named constants
- [ ] 5.5 Consider moving colors to a shared location or theme resources

## 6. Validation

- [ ] 6.1 Build and verify no compilation errors
- [ ] 6.2 Verify splash screen displays for ~2 seconds
- [ ] 6.3 Test theme switching still works
- [ ] 6.4 Verify RAM is still displayed correctly
- [ ] 6.5 Verify ping button colors still work correctly
