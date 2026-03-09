# Proposal: Add Loading States

## Summary

Implement skeleton placeholders and loading indicators to provide visual feedback during data loading operations.

## Motivation

Currently, the app shows a splash screen during initial load, then content appears all at once. This works but could feel abrupt. Adding skeleton placeholders or shimmer effects would:
- Provide smoother perceived performance
- Give users confidence that data is loading
- Follow modern UI patterns used in Windows 11 apps

Additionally, the refresh operation (if present) should show inline loading state rather than freezing the UI.

## Scope

### In Scope
- Skeleton placeholder cards during initial data load
- Inline loading indicator during refresh operations
- Shimmer effect on skeleton placeholders (optional)

### Out of Scope
- Changes to splash screen (keep as-is)
- Per-card refresh buttons
- Progress percentages or detailed loading messages

## Approach

1. **Skeleton Cards**: Create simplified card templates with placeholder rectangles instead of text
2. **Loading State**: Add `IsLoading` property to ViewModel, show skeletons when true
3. **Refresh Indicator**: Disable refresh button and show progress ring during refresh
4. **Shimmer Effect**: Optional gradient animation on placeholder rectangles

## Technical Considerations

- Skeleton templates should match card structure but use neutral placeholder shapes
- Loading state should be brief (system info loads quickly)
- Must work correctly in all themes
- Shimmer animation should be subtle, not distracting

## Impact

- **Files Changed**: ~5-10 files
- **New Components**: Skeleton card templates, loading overlay
- **Testing**: Performance testing, visual review, accessibility check
- **Risk**: Medium - affects perceived startup time, must not regress actual performance
