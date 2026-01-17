# Fix Theme Dropdown

## Problem Statement

The theme dropdown in the title bar does not work - selecting a theme has no visible effect on the UI.

## Root Cause Analysis

After investigating the code, several potential issues were identified:

1. **Event Timing Issue**: The `SelectionChanged` event in XAML fires during ComboBox initialization before `ThemeService` is fully set up, and the `_isInitializingTheme` flag may not properly prevent all unwanted early events.

2. **Theme Application Target**: Setting `RequestedTheme` on `RootGrid` may not properly propagate to all UI elements in WinUI 3, especially when the Window has already been rendered.

3. **Missing UI Refresh**: WinUI 3 sometimes requires explicit UI refresh after theme changes for the visual updates to take effect.

## Proposed Solution

1. **Remove XAML event binding** - Wire up the `SelectionChanged` event in code-behind after initialization to avoid timing issues.

2. **Apply theme at correct level** - Ensure the theme is applied to the Window's content root and propagates correctly.

3. **Add explicit refresh** - Force a visual refresh after theme changes to ensure the UI updates.

4. **Add logging** - Include debug logging to help diagnose any remaining issues.

## Scope

- `MainWindow.xaml` - Remove `SelectionChanged` attribute
- `MainWindow.xaml.cs` - Wire up event after initialization, improve theme application
- `ThemeService.cs` - Add logging and ensure robust theme application

## Risk Assessment

- **Low Risk**: Changes are localized to theme handling code
- **No Breaking Changes**: The fix maintains the same user-facing behavior (just makes it work)
- **Testable**: Can verify by selecting different themes and observing visual changes
