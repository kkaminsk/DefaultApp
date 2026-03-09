# Proposal: Low-Effort UI Polish

## Summary

Quick wins to improve visual consistency and user feedback without significant code changes.

## Motivation

The current UI is functional but has minor inconsistencies that affect polish:
- Trailing separator lines appear before action buttons and at card ends
- Copy buttons provide no visual feedback on success
- Empty state text varies between properties
- Icon-only buttons lack hover states

These are small fixes that individually take minimal effort but collectively improve the user experience.

## Scope

### In Scope
- Remove trailing separators before buttons and at card ends
- Add checkmark icon feedback when copy succeeds (1-second duration)
- Standardize empty state text to "N/A" across all properties
- Add hover background to copy button style

### Out of Scope
- Property grouping or card reorganization
- Loading states or skeleton placeholders
- Theme selector redesign
- Keyboard shortcuts

## Approach

1. **Trailing Separators**: Remove `Border` separators at specific locations in `MainPage.xaml`
2. **Copy Feedback**: Add a brief icon swap in the copy command handler, revert after 1 second
3. **Empty States**: Audit all service methods and ensure fallback text is "N/A"
4. **Hover States**: Update `CopyButtonStyle` to include `PointerOver` visual state with background

## Impact

- **Files Changed**: ~3-5 files
- **Testing**: Visual verification in all themes
- **Risk**: Low - cosmetic changes only
