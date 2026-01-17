## 1. Add Exception Logging to Empty Catch Blocks

- [x] 1.1 Update `SystemInfoService` catch blocks to log exceptions at Warning/Error level
- [x] 1.2 Update `HardwareInfoService` catch blocks to log exceptions at Warning/Error level
- [x] 1.3 Update `LoggingService` catch blocks to use Debug.WriteLine (can't use itself for logging)
- [x] 1.4 Review and update any remaining empty catch blocks in other services

## 2. Refactor Duplicate Ping Code

- [x] 2.1 Create private method `ExecutePingToAddressAsync(string address, Action<string> setButtonText, Action<Brush?> setBackground, string logName)`
- [x] 2.2 Update `ExecutePingAsync()` to call the common method for gateway ping
- [x] 2.3 Update `ExecutePingDnsAsync()` to call the common method for DNS ping
- [x] 2.4 Update `ExecutePingGoogleDnsAsync()` to call the common method for Google DNS ping
- [x] 2.5 Remove duplicated ping loop logic

## 3. Fix BiosInfoService Duplicate Return Values

- [x] 3.1 Investigate Registry for distinct BIOS name value (e.g., `BaseBoardProduct` or `SystemProductName`)
- [x] 3.2 Update `GetBiosName()` to return a distinct value or document the intentional duplication
- [x] 3.3 Alternatively, remove `GetBiosName()` if it provides no additional value over `GetBiosVersion()`

## 4. Fix ThemeService Memory Leak

- [x] 4.1 Implement `IDisposable` interface on `ThemeService`
- [x] 4.2 In `Dispose()`, unsubscribe from `_uiSettings.ColorValuesChanged`
- [x] 4.3 Update `MainWindow` to dispose `ThemeService` on window close
- [x] 4.4 Add disposed check to prevent operations after disposal

## 5. Validation

- [x] 5.1 Build and verify no compilation errors
- [x] 5.2 Test ping functionality still works correctly
- [x] 5.3 Verify exceptions are now logged in debug output
- [x] 5.4 Confirm BIOS info displays correctly
- [x] 5.5 Test theme switching still works
