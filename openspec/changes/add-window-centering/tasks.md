## 1. Implementation

- [x] 1.1 Add P/Invoke declarations for `GetMonitorInfo` and `MonitorFromWindow` to get display work area
- [x] 1.2 Add centering logic in `SetMinimumWindowSize` after resize to calculate center position
- [x] 1.3 Move window to calculated center position using `AppWindow.Move()`

## 2. Validation

- [ ] 2.1 Test on primary monitor - window should appear centered
- [x] 2.2 Build project to verify no compilation errors
