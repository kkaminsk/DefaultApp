## 1. Fix Fire-and-Forget Async Exception Handling

- [x] 1.1 Create a helper method in `MainViewModel` to safely execute fire-and-forget async operations with exception logging
- [x] 1.2 Update `PingGateway()` to use the safe async helper instead of `_ = ExecutePingAsync()`
- [x] 1.3 Update `PingDns()` to use the safe async helper instead of `_ = ExecutePingDnsAsync()`
- [x] 1.4 Update `PingGoogleDns()` to use the safe async helper instead of `_ = ExecutePingGoogleDnsAsync()`

## 2. Fix MainViewModel Disposal

- [x] 2.1 Add `Unloaded` event handler in `MainPage.xaml.cs` constructor
- [x] 2.2 Implement `OnUnloaded` method that calls `ViewModel.Dispose()`
- [x] 2.3 Verify MediaPlayer is properly disposed when page unloads

## 3. Fix Window Subclassing Cleanup

- [x] 3.1 Add `Closed` event handler in `MainWindow` constructor
- [x] 3.2 Implement cleanup method to restore `_oldWndProc` using `SetWindowLongPtr`
- [x] 3.3 Ensure window handle is valid before attempting cleanup

## 4. Validation

- [x] 4.1 Build application and verify no compilation errors
- [x] 4.2 Test ping functionality works correctly
- [x] 4.3 Verify no exceptions are thrown when closing the application
- [x] 4.4 Confirm memory usage does not grow after repeated page navigation (if applicable)
