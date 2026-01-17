## Why

The code review identified 3 critical issues that could cause application crashes, memory leaks, or silent failures:
1. Fire-and-forget async patterns without exception handling in ping commands
2. MainViewModel (which owns a MediaPlayer) is never disposed, causing resource leaks
3. Window subclassing delegate is not cleaned up on window close

## What Changes

- Add proper exception handling for fire-and-forget async calls in `MainViewModel`
- Implement dispose pattern for `MainViewModel` in `MainPage`
- Add cleanup for window subclassing in `MainWindow` to restore original WndProc

## Impact

- Affected code:
  - `DefaultApp/ViewModels/MainViewModel.cs` (lines 384, 455, 518)
  - `DefaultApp/Views/MainPage.xaml.cs`
  - `DefaultApp/MainWindow.xaml.cs` (lines 104-108)
- No breaking changes
- Improves stability and resource management
