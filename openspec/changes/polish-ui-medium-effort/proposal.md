# Proposal: Medium-Effort UI Improvements

## Summary

Accessibility and title bar improvements that require moderate code changes but significantly improve the user experience.

## Motivation

While the app has good accessibility foundations, some areas need attention:
- Focus indicators may not be visible in all themes (especially Cyberpunk)
- The title bar theme selector uses a full ComboBox which feels heavy
- The About window lacks visual separation between sections
- Icon-only buttons (info button) should have consistent hover states

## Scope

### In Scope
- Verify and fix focus indicator visibility in all themes
- Compact the theme selector (adjust styling or use flyout)
- Add separator and description to About window
- Extend hover state styling to info button

### Out of Scope
- Loading skeleton states
- Property grouping or card reorganization
- Keyboard shortcuts beyond current implementation

## Approach

1. **Focus Indicators**: Test each theme, add explicit `FocusVisualPrimaryBrush` overrides where needed
2. **Theme Selector**: Either reduce ComboBox width or replace with DropDownButton + MenuFlyout
3. **About Window**: Add horizontal separator, optional one-line app description
4. **Button Consistency**: Apply hover state styling to all icon-only buttons

## Impact

- **Files Changed**: ~5-8 files
- **Testing**: Accessibility testing with screen reader, visual review in all themes
- **Risk**: Low-Medium - UI changes with potential focus/keyboard navigation impact
