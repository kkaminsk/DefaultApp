## Why

The application currently opens at an arbitrary screen position determined by Windows, which may place it partially off-screen or in an inconvenient location. Centering the window on launch provides a consistent, professional user experience.

## What Changes

- Add window centering logic to position the app at the center of the primary display on launch
- Center calculation will account for display work area (excluding taskbar)
- Centering will respect DPI scaling

## Impact

- Affected specs: window-positioning (new capability)
- Affected code: `MainWindow.xaml.cs` (SetMinimumWindowSize method)
