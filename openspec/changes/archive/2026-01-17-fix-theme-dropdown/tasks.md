# Tasks

## Implementation

- [x] Remove `SelectionChanged` attribute from ThemeSelector ComboBox in MainWindow.xaml
- [x] Wire up SelectionChanged event in code-behind after InitializeThemeService completes
- [x] Add debug logging to ThemeSelector_SelectionChanged to trace event handling
- [x] Add debug logging to ThemeService.SetTheme and ApplyTheme methods
- [x] Test theme switching for all 6 themes (System Default, Light, Dark, Cyberpunk, High Contrast Dark, High Contrast Light)

## Verification

- [x] Build project and verify no compilation errors
- [x] Run application and verify theme dropdown opens
- [x] Select each theme and verify visual change occurs
- [x] Close and reopen app to verify theme persistence
- [x] Test System Default responds to Windows theme changes
